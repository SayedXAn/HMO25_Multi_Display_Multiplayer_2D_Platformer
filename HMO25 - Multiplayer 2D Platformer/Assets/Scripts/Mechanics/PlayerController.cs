using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public uint id;

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        public Collider2D collider2d;
        public AudioSource audioSource;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;
        private PlayerInput playerInput;

        public Bounds Bounds => collider2d.bounds;

        private Transform currentPlatform;
        private Vector3 lastPlatformPos;
        private Vector3 platformDelta;

        public GameObject spawnPoint;

        void Awake()
        {
            id = gameObject.GetComponent<PlayerInput>().user.id;
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            playerInput = GetComponent<PlayerInput>();

            // Bind actions from THIS player's input
            m_MoveAction = playerInput.actions["Move"];
            m_JumpAction = playerInput.actions["Jump"];
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = m_MoveAction.ReadValue<Vector2>().x;
                if (jumpState == JumpState.Grounded && m_JumpAction.WasPressedThisFrame())
                    jumpState = JumpState.PrepareToJump;
                else if (m_JumpAction.WasReleasedThisFrame())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }

            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            // animation should only care about the input movement
            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(move.x));

            // Add platform motion to actual velocity
            Vector2 platformVelocity = platformDelta / Time.deltaTime;
            targetVelocity = move * maxSpeed + platformVelocity;
        }




        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("movingplatform"))
            {
                currentPlatform = collision.transform;
                lastPlatformPos = currentPlatform.position;
            }
            
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform == currentPlatform)
            {
                currentPlatform = null;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("deathzone"))
            {
                //Debug.Log("Deaddddddddddddddddddddddd");
                StartCoroutine(AfterLife());
            }
        }

        IEnumerator AfterLife()
        {
            yield return new WaitForSeconds(1f);
            transform.position = spawnPoint.transform.position;
        }    
        void LateUpdate()
        {
            if (currentPlatform != null)
            {
                platformDelta = currentPlatform.position - lastPlatformPos;
                lastPlatformPos = currentPlatform.position;
            }
            else
            {
                platformDelta = Vector3.zero;
            }
        }



        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public void SwitchOnCamera()
        {
            // ?
        }
    }
}
