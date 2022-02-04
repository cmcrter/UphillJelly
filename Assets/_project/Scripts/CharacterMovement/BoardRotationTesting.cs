using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace L7Games.Movement
{
    public class BoardRotationTesting : MonoBehaviour
    {
        public Transform frontLeft;
        public Transform frontRight;
        public Transform backRight;
        public Transform backLeft;
        public Transform midRaycast;
        public float turnAxis;

        HisGroundBelow groundBelow = new HisGroundBelow();

        List<LineToDraw> linesToDraw;

        private struct LineToDraw
        {
            public Vector3 start;
            public Vector3 end;
            public Color color;

            public LineToDraw(Vector3 start, Vector3 end, Color color)
            {
                this.start = start;
                this.end = end;
                this.color = color;
            }
        }

        // Start is called before the first frame update
        void Update()
        {
            if (linesToDraw == null)
            {
                linesToDraw = new List<LineToDraw>();
            }
            // Do all the raycasts
            Physics.Raycast(frontLeft.transform.position, Vector3.down, out RaycastHit frontLeftHit, 10.0f  );
            Physics.Raycast(frontRight.transform.position, Vector3.down, out RaycastHit frontRightHit, 10.0f);
            Physics.Raycast(backRight.transform.position, Vector3.down, out RaycastHit backRightHit, 10.0f  );
            Physics.Raycast(backLeft.transform.position, Vector3.down, out RaycastHit backLeftHit, 10.0f    );
            linesToDraw.Add(new LineToDraw(frontLeft.transform.position, frontLeftHit.point, Color.white));
            linesToDraw.Add(new LineToDraw(frontRight.transform.position, frontRightHit.point, Color.white));
            linesToDraw.Add(new LineToDraw(backRight.transform.position, backRightHit.point, Color.white));
            linesToDraw.Add(new LineToDraw(backLeft.transform.position, backLeftHit.point, Color.white));

            // Calculate the roll
            // Find the angles between the width raycasts
            float frontAngle = Vector3.SignedAngle(Vector3.right, frontRightHit.point - frontLeftHit.point, Vector3.forward);
            //linesToDraw.Add(new LineToDraw(frontRightHit.point, frontLeftHit.point, Color.white));
            float backAngle = Vector3.SignedAngle(Vector3.right, backRightHit.point - backLeftHit.point, Vector3.forward);
            //linesToDraw.Add(new LineToDraw(backLeftHit.point, backRightHit.point, Color.white));

            frontAngle = CalculateSignedSlopeAngle(frontLeftHit.point, frontRightHit.point, Vector3.up, true);
            linesToDraw.Add(new LineToDraw(frontRightHit.point, frontLeftHit.point, Color.white));
            backAngle = CalculateSignedSlopeAngle(backLeftHit.point, backRightHit.point, Vector3.up, true);
            linesToDraw.Add(new LineToDraw(backLeftHit.point, backRightHit.point, Color.white));
            // Use the largest unsigned value
            float unsignedFrontAngle = frontAngle < 0f ? frontAngle * -1f : frontAngle;
            float unsignedBackAngle = backAngle < 0f ? backAngle * -1f : backAngle;
            float roll = unsignedFrontAngle > unsignedBackAngle ? frontAngle : backAngle;

            // Calculate the pitch
            // Find the angles between the length raycasts
            float leftAngle = CalculateSignedSlopeAngle(frontLeftHit.point, backLeftHit.point, Vector3.up, false);
            //linesToDraw.Add(new LineToDraw(frontLeftHit.point, backLeftHit.point, Color.white));
            float rightAngle = CalculateSignedSlopeAngle(frontRightHit.point, backRightHit.point, Vector3.up, false);
            //linesToDraw.Add(new LineToDraw(frontRightHit.point, backRightHit.point, Color.white));
            // Use the smallest unsigned value
            float unsignedLeftAngle = leftAngle < 0f ? leftAngle * -1f : leftAngle;
            float unsignedRightAngle = rightAngle < 0f ? rightAngle * -1f : rightAngle;
            float pitch = unsignedLeftAngle > unsignedRightAngle ? leftAngle : rightAngle;
            transform.rotation = Quaternion.Euler(new Vector3(pitch, transform.rotation.eulerAngles.y, roll));
            // Fronts
        }

        private void OnDrawGizmos()
        {
            if (linesToDraw == null)
            {
                linesToDraw = new List<LineToDraw>();
            }
            foreach (LineToDraw line in linesToDraw)
            {
                Gizmos.color = line.color;
                Gizmos.DrawLine(line.start, line.end);
            }
            linesToDraw.Clear();
        }

        private float CalculateSignedSlopeAngle(Vector3 startingPoint, Vector3 endPoint, Vector3 flatPlaneNormal, bool drawLines)
        {
            Vector3 slopeVector = endPoint - startingPoint;
            Vector3 flatVector = Vector3.ProjectOnPlane(slopeVector, flatPlaneNormal).normalized;
            if (drawLines)
            {
                linesToDraw.Add(new LineToDraw(startingPoint, startingPoint + flatVector, Color.green));
            }

            Vector3 rightFlatVector = Vector3.Cross(flatVector, flatPlaneNormal).normalized;
            if (drawLines)
            {
                linesToDraw.Add(new LineToDraw(startingPoint, startingPoint + rightFlatVector, Color.red));
            }
            return  Vector3.SignedAngle(flatVector, slopeVector, rightFlatVector);
        }
    }
}
