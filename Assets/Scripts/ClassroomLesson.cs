using UnityEngine;
namespace AulaInclusiva
{
    public class ClassroomLesson : MonoBehaviour
    {
        public FirstPersonController player;
        public int Page { get; private set; }
        public bool Completed { get; private set; }
        public bool Open { get; private set; }
        int fontSize = 24;
        readonly string[] titles = { "01 / Bienvenida", "02 / Comunicación inclusiva", "03 / Comprueba lo aprendido" };
        readonly string[] text = {
            "Todas las personas pueden aprender.\n\nEsta aula comunica las instrucciones mediante texto permanente. No necesitas escuchar ningún sonido.\n\nObjetivo: reconocer una forma de comunicación accesible.",
            "Mira a la persona al comunicarte.\n\nMantén el rostro visible y acompaña tus explicaciones con texto. Respeta el ritmo y la forma de comunicación que cada persona prefiera.\n\nLa lengua de señas tiene su propia gramática; este prototipo no enseña ni sustituye esa lengua.",
            "¿Qué ayuda a una persona con discapacidad auditiva a seguir una actividad?\n\nA. Dar instrucciones solo mediante audio.\nB. Presentar instrucciones claras por escrito." };
        string feedback = "";
        public bool CanInteract() {
            return Physics.Raycast(player.view.transform.position, player.view.transform.forward, out RaycastHit hit, 3.2f)
                && hit.collider.GetComponent<Blackboard>() != null;
        }
        public void Interact() { if (!Open && CanInteract()) { Open = true; player.Reading = true; player.Capture(false); } }
        public void Close() { Open = false; player.Reading = false; player.Capture(true); }
        public void Next() { Page = Mathf.Min(Page + 1, titles.Length - 1); }
        public void Answer(bool correct) { Completed = correct; feedback = correct ? "¡Correcto! Lección completada. Puedes volver a leerla." : "Inténtalo otra vez: el texto permite consultar las instrucciones sin audio."; }
        void Update() {
            if (Input.GetKeyDown(KeyCode.E)) { if (Open) Close(); else Interact(); }
            if (Open && Input.GetKeyDown(KeyCode.Escape)) Close();
        }
        void OnGUI() {
            float scale = Mathf.Min(Screen.width / 1100f, Screen.height / 760f);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * scale);
            float width = Screen.width / scale, height = Screen.height / scale;
            var label = new GUIStyle(GUI.skin.label) { fontSize = fontSize, wordWrap = true };
            var title = new GUIStyle(label) { fontSize = 30, fontStyle = FontStyle.Bold };
            var button = new GUIStyle(GUI.skin.button) { fontSize = 22, wordWrap = true };
            GUI.Box(new Rect(16, 16, width - 32, 112), "");
            GUI.Label(new Rect(32, 22, width - 64, 40), "AULA INCLUSIVA 3D", title);
            GUI.Label(new Rect(32, 65, width - 64, 55), "WASD: caminar · Ratón: mirar · E: pizarra · Esc: liberar cursor", label);
            if (!Open) {
                GUI.Label(new Rect(width / 2 - 8, height / 2 - 18, 30, 40), "+", label);
                GUI.Box(new Rect(16, height - 90, width - 32, 74), "");
                GUI.Label(new Rect(32, height - 80, width - 64, 65), CanInteract() ? "[E] Leer la pizarra" : "Acércate a la pizarra y apunta al centro. Toda la actividad es visual.", label);
                return;
            }
            float x = (width - 940) / 2;
            GUI.Box(new Rect(x, 145, 940, 580), "");
            GUI.Label(new Rect(x + 26, 165, 680, 45), titles[Page], title);
            if (GUI.Button(new Rect(x + 725, 165, 80, 42), "A−", button)) fontSize = Mathf.Max(22, fontSize - 2);
            if (GUI.Button(new Rect(x + 815, 165, 80, 42), "A+", button)) fontSize = Mathf.Min(30, fontSize + 2);
            GUI.Label(new Rect(x + 26, 225, 885, Page == 2 ? 245 : 415), text[Page], label);
            if (Page == 2) {
                if (GUI.Button(new Rect(x + 26, 482, 425, 52), "A · Solo audio", button)) Answer(false);
                if (GUI.Button(new Rect(x + 465, 482, 448, 52), "B · Instrucciones escritas", button)) Answer(true);
                GUI.Label(new Rect(x + 26, 545, 885, 90), feedback, label);
            }
            if (GUI.Button(new Rect(x + 26, 652, 200, 48), "Anterior", button)) Page = Mathf.Max(0, Page - 1);
            if (Page < 2 && GUI.Button(new Rect(x + 240, 652, 220, 48), "Siguiente", button)) Next();
            if (GUI.Button(new Rect(x + 690, 652, 220, 48), "Cerrar [E / Esc]", button)) Close();
        }
    }
}
