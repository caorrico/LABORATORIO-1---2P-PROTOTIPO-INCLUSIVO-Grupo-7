using UnityEngine;

namespace AulaInclusiva
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        public Camera view;
        public bool Reading { get; set; }
        public float speed = 3f;
        float pitch, vertical;
        CharacterController body;
        void Awake() { body = GetComponent<CharacterController>(); }
        void Start() { Capture(!Reading); }
        public void ResetPosition() {
            body.enabled = false; transform.SetPositionAndRotation(new Vector3(0,.05f,-4.8f), Quaternion.identity);
            view.transform.localRotation = Quaternion.identity; pitch = 0; vertical = 0; body.enabled = true;
        }
        public void Capture(bool capture) {
            Cursor.lockState = capture ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !capture;
        }
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) Capture(false);
            if (!Reading && Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0) Capture(true);
            if (!Reading && Cursor.lockState == CursorLockMode.Locked) {
                transform.Rotate(0, Input.GetAxis("Mouse X") * 2f, 0);
                pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * 2f, -75, 75);
                view.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
                Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), Time.deltaTime);
            } else Move(Vector2.zero, Time.deltaTime);
        }
        public void Move(Vector2 input, float dt) {
            if (body.isGrounded && vertical < 0) vertical = -2;
            vertical += Physics.gravity.y * dt;
            Vector3 direction = transform.right * input.x + transform.forward * input.y;
            body.Move((Vector3.ClampMagnitude(direction, 1) * speed + Vector3.up * vertical) * dt);
        }
        void OnDisable() { Capture(false); }
    }
}
