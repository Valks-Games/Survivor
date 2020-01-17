using System.Collections.Generic;
using UnityEngine;
using WorldAPI.Items;
using WorldAPI.Tasks;

namespace WorldAPI.Entities
{
    public partial class Entity<TType> : TaskedBehaviour<TType> where TType : Entity<TType>
    {
        public int Health = 1;
        public int MaxHealth = 1;
        public int Damage = 1;
        public float Speed = 100f;
        public float InteractionRange = 0.5f;

        public float WalkDrag = 1.0f;
        public float HaltDrag = 1.75f;
        public Vector3? Target = null;

        public readonly Inventory Inventory = new Inventory(maxSize: 3);

        protected Rigidbody rb;

        public void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            TaskLoop();
            Move();
        }

        private void Move()
        {
            if (Target != null && !IsAt((Vector3)Target))
            {
                rb.drag = WalkDrag;
                rb.AddForce(((Vector3)Target - transform.position).normalized * Speed * Time.deltaTime);
                Vector3 newDir = Vector3.RotateTowards(transform.position, (Vector3)Target, Speed * Time.deltaTime, 0.0f);
                if (rb.velocity.sqrMagnitude != 0)
                {
                    transform.rotation = Quaternion.LookRotation(newDir);
                }

                return;
            }

            rb.drag = HaltDrag;
            Target = null;
        }

        public void MoveTowards(Vector3 target)
        {
            Target = target;
        }

        public void MoveTowards<TTarget>(TTarget target) where TTarget : Component
        {
            Target = target.transform.position;
        }

        public void TeleportTo(Vector3 target)
        {
            transform.position = target;
        }

        public void TeleportTo<TTarget>(TTarget target) where TTarget : Component
        {
            transform.position = target.transform.position;
        }

        public bool IsAt(Vector3 target)
        {
            return (target - transform.position).sqrMagnitude <= InteractionRange * InteractionRange;
        }

        public bool IsAt<TTarget>(TTarget target) where TTarget : Component
        {
            return IsAt(target.transform.position);
        }

        public TTarget Nearest<TTarget>(List<TTarget> targets = null) where TTarget : Component
        {
            TTarget closest = null;
            float minimumDistance = Mathf.Infinity;

            foreach (TTarget target in targets ?? new List<TTarget>(FindObjectsOfType<TTarget>()))
            {
                if (target.transform == transform)
                    continue;

                float distance = Vector2.Distance(transform.position, target.transform.position);

                if (distance < minimumDistance)
                {
                    closest = target;
                    minimumDistance = distance;
                }
            }

            return closest;
        }

        public override string ToString()
        {
            return "A entity.";
        }
    }

    public partial class Entity<TType> : TaskedBehaviour<TType>
    {
        public static new Entity New(string name = "Entity")
        {
            return Derive(name).AddComponent<Entity>();
        }

        protected static new GameObject Derive(string name = "Entity")
        {
            GameObject entity = TaskedBehaviour<Entity>.Derive(name);

            entity.AddComponent<Rigidbody2D>();

            return entity;
        }
    }

    public class Entity : Entity<Entity> { }
}