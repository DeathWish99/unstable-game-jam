using UnityEditor;
using UnityEngine;

namespace Bullet
{
    public class BulletBase : MonoBehaviour
    {
        public enum BulletType
        {
            Straight,
            Curved,
            ZigZag
        }
        public BulletType type;
        [SerializeField]
        private int angle = 0;
        
        [SerializeField]
        private float bulletLife = 1f;
        [SerializeField]
        private float speed = 1f;
        
        //Zig Zag
        [HideInInspector]
        public float magnitude = 0f; //Size of sine movement
        [HideInInspector]
        public float frequency = 0f; //Speed of sine movement

        //Curved
        [HideInInspector]
        [Tooltip("Controls the direction/speed the spiral translates")]
        public Vector2 linearVelocity = new Vector2(0.5f, 0f);
    
        [HideInInspector]
        [Tooltip("Controls the radius of the circular motion for X/Y axis.")]
        public Vector2 radii = new Vector2(2f, 3f);
        
        private Vector2 spawnPoint;
        private Vector2 spawnDirection;
        private float timer;
        
        public bool isTypeZigZagOrCurved => type == BulletType.ZigZag || type == BulletType.Curved;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spawnPoint = transform.position;
            spawnDirection = transform.up;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            transform.position = Movement();
        }

        private Vector2 Movement()
        {
            float x = timer * speed * spawnDirection.x;
            float y = timer * speed * spawnDirection.y;
            switch (type)
            {
                case BulletType.Straight:
                    return new Vector2(x+spawnPoint.x, y+spawnPoint.y);
                case BulletType.Curved:
                    var curveAngle = speed * timer;
                    var pos = linearVelocity * timer;
                    return pos + new Vector2(Mathf.Cos(curveAngle) * radii.x, Mathf.Sin(curveAngle) * radii.y);
                case BulletType.ZigZag:
                    return new Vector2(Mathf.Sin(Time.time * frequency) * magnitude, y+spawnPoint.y);
                default:
                    return Vector2.zero;
            }
        }

        public void SetRotation(int angleParam)
        {
            this.angle = angleParam;
        }
    }

    [CustomEditor(typeof(BulletBase))]
    public class Bullet_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (BulletBase)target;

            if (!script.isTypeZigZagOrCurved)
                return;
            
            if(script.type == BulletBase.BulletType.ZigZag)
            {
                script.magnitude = EditorGUILayout.FloatField("Magnitude", script.magnitude);
                script.frequency = EditorGUILayout.FloatField("Frequency", script.frequency);
                
            }
            else
            {
                script.linearVelocity = EditorGUILayout.Vector2Field("Linear Velocity", script.linearVelocity);
                script.radii = EditorGUILayout.Vector2Field("Radii", script.radii);
            }
        }
    }
}