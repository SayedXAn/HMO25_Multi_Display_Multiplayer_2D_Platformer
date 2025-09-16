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
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public uint id;
        //public GameObject myCam;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;

        public Bounds Bounds => collider2d.bounds;

        private PlayerInput playerInput;
        public bool onMovingPlatform = false;
        public GameObject movingPlatformRef;
        public float gap = 0f;
        public GameObject parent;

        void Awake()
        {
            id = gameObject.GetComponent<PlayerInput>().user.id;
            health = GetComponent<Health>();
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

                //if (onMovingPlatform && m_MoveAction.ReadValue<Vector2>().x != 0f)
                //{
                //    //Debug.Log("Gap is  " + gap);
                //    //float temp = transform.position.x + gap + m_MoveAction.ReadValue<Vector2>().x;

                //    //if (gap > 5)
                //    //{
                //    //    onMovingPlatform = false;
                //    //    temp = movingPlatformRef.transform.position.x + 1.5f;
                //    //}
                //    //if (gap < -5)
                //    //{
                //    //    onMovingPlatform = false;
                //    //    temp = movingPlatformRef.transform.position.x - 1.5f;
                //    //}
                //    //0.366932
                //    //float temp = transform.position.x + move.x;
                //    //transform.position = new Vector3(temp, transform.position.y, 0);
                //    move.x = m_MoveAction.ReadValue<Vector2>().x * 20;

                //    //Debug.Log("player pos: " + transform.position.x + " Temp is  " + temp + " movingPlatformRef.transform.position.x " + movingPlatformRef.transform.position.x + " m_MoveAction.ReadValue<Vector2>().x " + m_MoveAction.ReadValue<Vector2>().x);
                //}
            }
            else
            {
                move.x = 0;                
            }
            UpdateJumpState();
            base.Update();
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if(collision.gameObject.tag == "movingplatform" )
        //    {
        //       onMovingPlatform = true;
        //       //movingPlatformRef = collision.gameObject;
        //       //gap = collision.gameObject.transform.position.x - transform.position.x;
        //       transform.parent = collision.transform;
        //    }
        //}
        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.tag == "movingplatform")
        //    {
        //        onMovingPlatform = false;
        //        movingPlatformRef = null;
        //        transform.parent = parent.transform;
        //    }
        //}
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

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
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