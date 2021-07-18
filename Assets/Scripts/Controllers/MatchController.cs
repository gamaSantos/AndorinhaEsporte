using System;
using AndorinhaEsporte.Domain;
using AndorinhaEsporte.Services;
using UnityEngine;

namespace AndorinhaEsporte.Controller
{
    public class MatchController : MonoBehaviour
    {
        private MatchStats _matchStats;
        private Match _match;
        private HudController _hudController;
        private CameraController _camera;
        private BallController _ball;


        private Guid LastTeamWithServe;

        private bool isWaitingRotationToServe;
        private Team TeamWaitingToServe;

        public bool isInPlay = false;

        [SerializeField]
        GameObject playerPrefab;

        void Start()
        {
            _ball = GameObject.FindObjectOfType<BallController>();
            _camera = Camera.main.GetComponent<CameraController>();
            initiateMatch();
            _hudController = GameObject.FindObjectOfType<HudController>();
        }

        void Update()
        {
            _hudController.UpdateHud(_matchStats);
            if (_matchStats.IsFinished)
            {
                Time.timeScale = 0;
                _hudController.ShowEndScreen();
                return;
            }

            if (_match.IsServing())
            {
                var player = _match.GetServingPlayer();
                if (player != null)
                {
                    var team = _match.GetTeam(player.TeamId);
                    if (team.HasControllerAssociated) _camera.MoveToServeAngle(player.Position);
                }
            }
            if (isInPlay)
            {
                _camera.MoveToMainAngle();
            }
        }
        void FixedUpdate()
        {
            if (isInPlay || _match.IsTeamBusy()) return;

            if (_matchStats.IsSetFinished)
            {
                _match.ChangeSides();
                _camera.ChangeSides();
                return;
            }

            if (!_camera.InTransition) _match.ApproveServe();
            if (isWaitingRotationToServe)
            {
                AllowServe();
                return;
            }
            if (!_matchStats.IsSetFinished && !_match.IsServing() && !isInPlay)
            {
                AllowServe();
                return;
            }



            if (_match.FinishedServe())
            {
                isInPlay = true;
            }

        }

        private void AllowServe()
        {
            if (_match.HomeTeam.IsResetingPosition() || _match.AwayTeam.IsResetingPosition())
            {
                return;
            }

            isWaitingRotationToServe = false;
            StartServe();
            TeamWaitingToServe = null;
            return;
        }

        private void initiateMatch()
        {
            _match = MatchService.Current;
            _matchStats = _match.Stats;
            CreateTeam(_match.HomeTeam);
            CreateTeam(_match.AwayTeam);
            _ball.SetMatchController(this);
            TeamWaitingToServe = _match.AwayTeam;
            _ball.ChangedDirection += _match.Stats.CountTouches;
        }

        private void CreateTeam(Team team)
        {
            foreach (var player in team.Formation)
            {
                var playerObject = GameObject.Instantiate(playerPrefab);
                playerObject.transform.position = player.FieldPosition.GetStartPosition(team.Foward);
                playerObject.transform.forward = team.Foward;
                var behavior = playerObject.GetComponent<PlayerController>();
                behavior.Initiate(player, team);
            }
            _ball.ChangedDirection += team.CheckBall;
        }

        internal void AddScore(Vector3 ballPosition, Guid lastContactTeamId)
        {
            if (!isInPlay) return;
            var scoreTeamId = lastContactTeamId;


            if (!ballPosition.IsInPlayField() || !ballPosition.IsInCourt())
            {
                scoreTeamId = _match.GetOponnentId(scoreTeamId);
            }
            else
            {
                scoreTeamId = _match.GetOponnentId(_match.GetTeamIdFromContactPoint(ballPosition));
            }

            var scoreTeam = _match.HomeTeam.Id == scoreTeamId ? _match.HomeTeam : _match.AwayTeam;
            var otherTeam = _match.HomeTeam.Id != scoreTeamId ? _match.HomeTeam : _match.AwayTeam;
            isInPlay = false;
            scoreTeam.ResetState();
            otherTeam.ResetState();
            var isHomeTeam = scoreTeam.Id == _match.HomeTeam.Id;
            _matchStats.AddScore(isHomeTeam);

            TeamWaitingToServe = scoreTeam;
            if (_matchStats.IsSetFinished) return;

            if (LastTeamWithServe != scoreTeam.Id)
            {
                scoreTeam.Rotate();
            }

            otherTeam.ResetPosition();


            isWaitingRotationToServe = true;
        }


        public void StartServe()
        {
            if (TeamWaitingToServe == null) return;
            LastTeamWithServe = TeamWaitingToServe.Id;
            _matchStats.StartedServe(LastTeamWithServe);
            TeamWaitingToServe.Serve();
        }

    }
}