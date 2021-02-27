using System.Collections;
using System.Collections.Generic;
using AndorinhaEsporte.Domain;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace AndorinhaEsporte.Controller
{
    public class PhysicsSceneController : MonoBehaviour
    {
        Scene _activeScene;
        private PhysicsScene _physicsScene;

        Scene _simulationScene;
        private PhysicsScene _simulationPhysicsScene;
        private const int DEFAULT_MAX_ITERATIONS = 200;


        // Start is called before the first frame update
        void Start()
        {
            _activeScene = SceneManager.GetActiveScene();
            _physicsScene = _activeScene.GetPhysicsScene();

            var sceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            _simulationScene = SceneManager.CreateScene("Simulation", sceneParameters);
            _simulationPhysicsScene = _simulationScene.GetPhysicsScene();
        }


        void FixedUpdate()
        {
            if (_physicsScene.IsValid() && !Physics.autoSimulation)
            {
                _physicsScene.Simulate(Time.deltaTime);
            }
        }

        public void SimulateLandingSpot(GameObject gameObject, Vector3 force, Vector3 torque)
        {
            GameObject simObject = null;
            try
            {
                var simData = PrepareSimulation(gameObject);
                if (simData == null) return;
                var ball = simData.Ball;
                simObject = simData.SimObject;
                ball.Trajectory = SimulateTrajectory(force, torque, simData, 0.2f);
            }
            finally
            {
                if (simObject != null) Destroy(simObject);
                Physics.autoSimulation = true;
            }
        }

        public float TryGetNeededForce(GameObject gameObject, Vector3 startPosition, Vector3 target, Vector3 direction)
        {
            GameObject simObject = null;
            float force = 0f;
            float minDistance = 1000;
            try
            {
                var simData = PrepareSimulation(gameObject);
                simObject = simData.SimObject;
                var simRigidBody = simData.SimRigidBody;
                var simForce = 2f;
                var simForceStep = 0.2f;
                for (var i = 0; i <= 10; i++)
                {
                    simObject.transform.position = startPosition;
                    var forceVector = direction * simForce;
                    var trajectory = SimulateTrajectory(force: forceVector, torque: forceVector * -1, simData, target.y);
                    var distance = target.Distance(trajectory.Last);
                    if (distance < minDistance)
                    {
                        force = simForce;
                        minDistance = distance;
                    }
                    if (distance < 0.2f) return force;
                    simForce += simForceStep;
                }
                return force;
            }
            finally
            {
                if (simObject != null) Destroy(simObject);
                Physics.autoSimulation = true;
            }
        }

        private Trajectory SimulateTrajectory(Vector3 force, Vector3 torque, SimulationBaseData simData, float targetHeight)
        {
            var simObject = simData.SimObject;
            var rigidBody = simData.SimRigidBody;
            var trajectory = new Trajectory();
            rigidBody.velocity = simData.OriginalRigidBody.velocity;
            rigidBody.angularVelocity = simData.OriginalRigidBody.angularVelocity;
            rigidBody.useGravity = true;
            rigidBody.AddForce(force);
            rigidBody.AddTorque(torque);

            for (int i = 0; i <= DEFAULT_MAX_ITERATIONS; i++)
            {
                _simulationPhysicsScene.Simulate(Time.fixedDeltaTime);
                var position = simObject.transform.position;
                var goingDownwards = rigidBody.velocity.y < 0;
                trajectory.Add(position);
                if (goingDownwards && position.y <= targetHeight) return trajectory;
            }
            return trajectory;
        }
        private SimulationBaseData PrepareSimulation(GameObject gameObject)
        {
            Rigidbody originalRigidBody;
            BallController ball;
            if (!gameObject.TryGetComponent<Rigidbody>(out originalRigidBody))
            {
                Physics.autoSimulation = true;
                return null;
            }
            if (!gameObject.TryGetComponent<BallController>(out ball))
            {
                Physics.autoSimulation = true;
                return null;
            }
            Physics.autoSimulation = false;
            var simulationObject = Instantiate(gameObject);
            simulationObject.transform.position = gameObject.transform.position;

            SceneManager.MoveGameObjectToScene(simulationObject, _simulationScene);

            var rigidBody = simulationObject.GetComponent<Rigidbody>();
            return new SimulationBaseData(originalRigidBody, ball, simulationObject, rigidBody);
        }

        private class SimulationBaseData
        {
            public SimulationBaseData(Rigidbody originalRigidBody, BallController ball, GameObject simObject, Rigidbody simRigidBody)
            {
                OriginalRigidBody = originalRigidBody;
                Ball = ball;
                SimObject = simObject;
                SimRigidBody = simRigidBody;
            }

            public Rigidbody OriginalRigidBody { get; }
            public BallController Ball { get; }
            public GameObject SimObject { get; }
            public Rigidbody SimRigidBody { get; }
        }
    }

}
