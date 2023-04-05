using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _moveThreshold = .05f;
    [SerializeField] [Range(0, 1)] float _snapiness = .8f;
    [SerializeField] [Range(0, 1)] float _rotationSnapiness = .6f;
    [SerializeField] Transform _rotationTransform;

    float _currentSpeed = 0f;

    [Header("Dash")]
    [SerializeField] float _dashSpeedMultiplier = 2.5f;
    [SerializeField] float _dashTime = .5f;
    [SerializeField] float _dashCooldown = .6f;

    Vector3 _dashDirection;
    float _dashTimer = 0f;
    float _dashCooldownTimer = 0f;

    [Header("Jump")]
    [SerializeField] float _jumpHeight = 2f;
    [SerializeField] float _jumpCooldown = .2f;
    [SerializeField] float _gravityMultiplier = 2f;

    float _jumpCooldownTimer = 0f;
    float _yVelocity = 0f;

    [Header("Ground check")]
    [SerializeField] float _groundCheckRadius = .25f;
    [SerializeField] Transform _groundCheckOrigin;
    [SerializeField] LayerMask _groundLayerMask;

    bool _isGrounded = true;

    [Space]
    [SerializeField] float _relativeAngle = -45f;

    [Header("Components")]
    public bool MovementEnabled = true;
    public bool DashEnabled = true;
    public bool JumpEnabled = true;
    public bool RotationEnabled = true;

    CharacterController _characterController;

    #region Input

    Vector2 _movementInput => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

    bool _isMoving => Mathf.Abs(_movementInput.x) > _moveThreshold || Mathf.Abs(_movementInput.y) > _moveThreshold;

    bool _jumpInput => Input.GetButtonDown("Jump");

    bool _dashInput => Input.GetButtonDown("Fire1");

    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _dashDirection = _rotationTransform.forward;
    }

    void Update()
    {
        UpdateDash();
        UpdateMovement();
        UpdateRotation();
        UpdateGrounded();
        UpdateJump();
    }

    void UpdateMovement()
    {
        if (MovementEnabled && (_isMoving || _dashCooldownTimer > 0f))
        {
           _currentSpeed = Mathf.Lerp(_currentSpeed, _moveSpeed, _snapiness * Time.deltaTime) * (_dashTimer > 0f ? _dashSpeedMultiplier : 1f); // if dashing - multiply speed by dash multiplier

            Vector2 direction = _dashCooldownTimer > 0f ? _dashDirection : _movementInput; // use dash direction if we are dashing

            Vector2 motion = Quaternion.Euler(0f, 0f, _relativeAngle) * direction * _currentSpeed * Time.deltaTime;

            _characterController.Move(new Vector3(motion.x, 0f, motion.y));
        }
        else
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, _snapiness * Time.deltaTime);
    }

    void UpdateDash()
    {
        _dashCooldownTimer -= Mathf.Min(_dashCooldownTimer, Time.deltaTime);
        _dashTimer -= Mathf.Min(_dashTimer, Time.deltaTime);

        if (_dashTimer <= 0f && _isMoving) // update dash direction only if we are not dashing
            _dashDirection = _movementInput;

        if (!DashEnabled || _dashCooldownTimer > 0f || _dashTimer > 0f)
            return;

        if (_dashInput)
        {
            _dashTimer += _dashTime;
            _dashCooldownTimer += _dashCooldown;
        }
    }

    void UpdateJump()
    {
        _jumpCooldownTimer -= Mathf.Min(_jumpCooldownTimer, Time.deltaTime);

        if (JumpEnabled && _jumpInput && _isGrounded && _jumpCooldownTimer <= 0)
            ApplyJump(_jumpHeight);

        _characterController.Move(Vector3.up * _yVelocity * Time.deltaTime);
    }

    void UpdateRotation()
    {
        if (!RotationEnabled || !_isMoving)
            return;

        var lookVector = _relativeAngle * new Vector3(_movementInput.x, 0f, _movementInput.y);
        var targetRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        _rotationTransform.localRotation = targetRotation;
    }

    void UpdateGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckOrigin.position, _groundCheckRadius, _groundLayerMask, QueryTriggerInteraction.Ignore);

        if (_isGrounded && _yVelocity < 0f)
            _yVelocity = -2f;
        else
            _yVelocity += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
    }

    public void ApplyJump(float distance)
    {
        _yVelocity = GetJumpForce(distance);
        _jumpCooldownTimer += _jumpCooldown;
    }

    float GetJumpForce(float distance)
    {
        return Mathf.Sqrt(distance * -2 * Physics.gravity.y * _gravityMultiplier);
    }
}