using System;
using UnityEngine;
namespace AulaInclusiva
{
    public class ClassroomLesson : MonoBehaviour
    {
        public FirstPersonController player;
        public bool InMenu { get; private set; } = true;
        public bool Open { get; private set; }
        public int LevelIndex { get; private set; }
        public int CardIndex { get; private set; }
        public int QuestionIndex { get; private set; }
        public int Score { get; private set; }
        public bool Completed { get; private set; }
        public bool ExamStarted { get; private set; }
        public bool PracticePassed { get; private set; }
        public int[] Answers { get; private set; }
        public int[] BestScores { get; private set; } = new[] { -1, -1, -1 };
        public StudyLevel Level => EcuadorContent.Levels[LevelIndex];
        public bool ReadyForExam {
            get { if (!PracticePassed) return false; foreach (bool seen in visited) if (!seen) return false; return true; }
        }
        bool[] visited = new bool[0];
        int selected = -1, fontSize = 24;
        string feedback = "";
        Vector2 scroll;
        Texture2D panel, buttonTexture, hoverTexture, accentTexture;
        GUIStyle label, title, button, small;
        Texture2D[][] illustrations;
        void Awake() { player.Reading = true; }
        void Start() { player.Capture(false); }
        public bool CanInteract() {
            return !InMenu && Physics.Raycast(player.view.transform.position, player.view.transform.forward, out RaycastHit hit, 3.2f)
                && hit.collider.GetComponent<Blackboard>() != null;
        }
        public void EnterLevel(int index) {
            if (index < 0 || index >= EcuadorContent.Levels.Length) return;
            LevelIndex = index; InMenu = false; Open = false;
            visited = new bool[Level.Cards.Length]; CardIndex = 0; visited[0] = true;
            PracticePassed = false; feedback = ""; ResetExam();
            player.ResetPosition(); player.Reading = false; player.Capture(true);
        }
        public void ReturnToMenu() { InMenu = true; Open = false; player.Reading = true; player.Capture(false); scroll = Vector2.zero; }
        public void Interact() { if (!Open && CanInteract()) { Open = true; player.Reading = true; player.Capture(false); scroll = Vector2.zero; } }
        public void Close() { Open = false; player.Reading = InMenu; player.Capture(!InMenu); }
        public void SelectCard(int index) {
            if (index < 0 || index >= Level.Cards.Length) return;
            CardIndex = index; visited[index] = true; feedback = "";
        }
        public void AnswerPractice(int answer) {
            PracticePassed = answer == Level.Practice.Correct;
            feedback = (PracticePassed ? "CORRECTO. " : "PRUEBA OTRA VEZ. ") + Level.Practice.Explanation;
        }
        void ResetExam() {
            ExamStarted = false; Completed = false; QuestionIndex = 0; Score = 0; selected = -1; scroll = Vector2.zero;
            Answers = new int[Level.Exam.Length]; for (int i = 0; i < Answers.Length; i++) Answers[i] = -1;
        }
        public void StartExam() { if (!ReadyForExam) return; ResetExam(); ExamStarted = true; }
        public void SubmitAnswer(int answer) {
            if (!ExamStarted || Completed || Answers[QuestionIndex] >= 0 || answer < 0 || answer >= Level.Exam[QuestionIndex].Options.Length) return;
            Answers[QuestionIndex] = answer;
            if (answer == Level.Exam[QuestionIndex].Correct) Score++;
        }
        public void NextQuestion() {
            if (!ExamStarted || Completed || Answers[QuestionIndex] < 0) return;
            if (QuestionIndex < Level.Exam.Length - 1) { QuestionIndex++; selected = -1; scroll = Vector2.zero; }
            else { Completed = true; BestScores[LevelIndex] = Mathf.Max(BestScores[LevelIndex], Score); scroll = Vector2.zero; }
        }
        void Update() {
            if (Input.GetKeyDown(KeyCode.M)) ReturnToMenu();
            if (InMenu) return;
            if (Input.GetKeyDown(KeyCode.E)) { if (Open) Close(); else Interact(); }
            if (Open && Input.GetKeyDown(KeyCode.Escape)) Close();
        }
        Texture2D Solid(Color color) { var t = new Texture2D(1,1); t.SetPixel(0,0,color); t.Apply(); return t; }
        void Styles() {
            if (!panel) {
                illustrations = new Texture2D[EcuadorContent.Levels.Length][];
                for (int l=0;l<illustrations.Length;l++) {
                    illustrations[l] = new Texture2D[EcuadorContent.Levels[l].Cards.Length];
                    for (int c=0;c<illustrations[l].Length;c++) illustrations[l][c] = LearningIllustrations.Create(l,c);
                }
                panel = Solid(new Color(.035f,.05f,.075f,1));
                buttonTexture = Solid(new Color(.13f,.19f,.26f,1));
                hoverTexture = Solid(new Color(.22f,.31f,.40f,1));
                accentTexture = Solid(new Color(.04f,.32f,.37f,1));
            }
            label = new GUIStyle(GUI.skin.label) { fontSize = fontSize, wordWrap = true, richText = false, padding = new RectOffset(4,4,5,5) };
            label.normal.textColor = Color.white;
            title = new GUIStyle(label) { fontSize = 30, fontStyle = FontStyle.Bold };
            small = new GUIStyle(label) { fontSize = 20 };
            button = new GUIStyle(GUI.skin.button) { fontSize = fontSize, wordWrap = true, padding = new RectOffset(14,14,12,12), margin = new RectOffset(4,4,4,4), richText = false };
            button.normal.background = buttonTexture; button.normal.textColor = Color.white;
            button.hover.background = hoverTexture; button.hover.textColor = Color.white;
            button.active.background = accentTexture; button.active.textColor = Color.white;
            button.focused.background = hoverTexture; button.focused.textColor = Color.white;
            button.onNormal = button.active; button.onHover = button.hover; button.onActive = button.active; button.onFocused = button.focused;
        }
        void Text(string value) { GUILayout.Label(value, label); }
        bool Button(string value) {
            float width = Screen.width / Mathf.Min(Screen.width/1100f,Screen.height/760f);
            float available = InMenu ? (width-96)/3-8 : width-112;
            return GUILayout.Button(value, button, GUILayout.Height(Mathf.Max(64,button.CalcHeight(new GUIContent(value),available)+8)));
        }
        void Banner(string value) {
            var style = new GUIStyle(label); style.normal.background = accentTexture; style.padding = new RectOffset(16,16,14,14);
            GUILayout.Label(value, style);
        }
        void OnGUI() {
            Styles();
            float scale = Mathf.Min(Screen.width / 1100f, Screen.height / 760f);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * scale);
            float width = Screen.width / scale, height = Screen.height / scale;
            GUI.color = Color.white;
            if (InMenu || Open) GUI.DrawTexture(new Rect(0,0,width,height), panel);
            else GUI.DrawTexture(new Rect(16,16,width-32,110), panel);
            GUI.Label(new Rect(32,22,width-250,48), "AULA INCLUSIVA · ECUADOR", title);
            GUI.Label(new Rect(32,70,width-260,45), InMenu ? "Elige uno de los 3 niveles antes de entrar al aula." : "Nivel " + (LevelIndex+1) + " · " + Level.Title, small);
            if (InMenu || Open) {
                if (GUI.Button(new Rect(width-214,30,80,50), "A−",button)) fontSize = Mathf.Max(22,fontSize-2);
                if (GUI.Button(new Rect(width-124,30,80,50), "A+",button)) fontSize = Mathf.Min(32,fontSize+2);
            }
            if (!InMenu && !Open) {
                GUI.Label(new Rect(width/2-8,height/2-18,30,40), "+",label);
                GUI.DrawTexture(new Rect(16,height-125,width-32,109), panel);
                GUI.Label(new Rect(32,height-120,width-64,100), (CanInteract() ? "[E] Abrir clase y examen" : "Acércate a la pizarra y apunta al centro.") + "\nWASD: caminar · Ratón: mirar · Esc: cursor · M: menú",label);
                return;
            }
            GUILayout.BeginArea(new Rect(32,130,width-64,height-230));
            scroll = GUILayout.BeginScrollView(scroll);
            if (InMenu) DrawMenu();
            else if (Completed) DrawResults();
            else if (ExamStarted) DrawExam();
            else DrawClass();
            GUILayout.Space(24); GUILayout.EndScrollView(); GUILayout.EndArea();
            GUI.DrawTexture(new Rect(0,height-86,width,86),panel);
            if (InMenu) GUI.Label(new Rect(36,height-80,width-72,75), "Todo el contenido es visual. Sin audio, sin tiempo límite. Usa la rueda para desplazarte.\nA− / A+: texto. Los resultados se conservan durante esta sesión.",small);
            else {
                if (GUI.Button(new Rect(32,height-75,245,58), "Volver al menú [M]",button)) ReturnToMenu();
                if (GUI.Button(new Rect(width-300,height-75,268,58), "Volver al aula [E]",button)) Close();
                GUI.Label(new Rect(300,height-70,width-625,65), "Sin cronómetro.\nDesplázate con la rueda.",small);
            }
        }
        void DrawMenu() {
            Banner("Aprende a tu ritmo: elige un tema, explora sus fichas y completa su examen final.");
            GUILayout.BeginHorizontal();
            for (int i=0;i<EcuadorContent.Levels.Length;i++) {
                GUILayout.BeginVertical(GUILayout.Width((Screen.width / Mathf.Min(Screen.width/1100f,Screen.height/760f)-96)/3));
                GUILayout.Label(illustrations[i][0],GUILayout.Height(112));
                GUILayout.Space(12); var level = EcuadorContent.Levels[i];
                GUILayout.Label("NIVEL " + (i+1) + " / " + level.Title,title);
                Text(level.Goal + "  " + level.Cards.Length + " fichas + práctica + 3 preguntas.");
                Text(BestScores[i] < 0 ? "Aún sin examen completado." : "Mejor score de esta sesión: " + BestScores[i] + "/3 · " + Mathf.RoundToInt(BestScores[i]*100f/3) + "%");
                if (Button("Entrar · Nivel " + (i+1))) EnterLevel(i);
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal(); GUILayout.Space(16);
            Text("Diseñado para aprender sin depender del sonido: instrucciones escritas, respuestas visibles y avance manual. No incluye traducción a lengua de señas ecuatoriana.");
        }
        void DrawClass() {
            GUILayout.Label("CLASE INTERACTIVA · " + Level.Title,title);
            Text(Level.Goal + " Selecciona cada ficha para descubrir la información.");
            for (int i=0;i<Level.Cards.Length;i++)
                if (Button((visited[i] ? "[VISTA] " : "[EXPLORAR] ") + Level.Cards[i].Title + (CardIndex==i ? " · seleccionada" : ""))) SelectCard(i);
            GUILayout.Space(12);
            GUILayout.Label(illustrations[LevelIndex][CardIndex],GUILayout.Height(144));
            GUILayout.Label("Ilustración esquemática · no es un mapa ni una seña",small);
            Banner(Level.Cards[CardIndex].Key);
            GUILayout.Label(Level.Cards[CardIndex].Title,title); Text(Level.Cards[CardIndex].Text);
            GUILayout.Space(16); GUILayout.Label("PRACTICA SIN PUNTUACIÓN",title);
            DrawOptions(Level.Practice, AnswerPractice);
            if (feedback.Length>0) Banner(feedback);
            Text(ReadyForExam ? "Clase lista. El examen tiene 3 preguntas; cada acierto vale 1 punto." : "Para abrir el examen, visita todas las fichas y resuelve la práctica. Puedes reintentar.");
            GUI.enabled = ReadyForExam; if (Button("Comenzar examen final")) StartExam(); GUI.enabled = true;
        }
        void DrawOptions(QuizQuestion question, Action<int> answer) {
            Text(question.Prompt);
            for (int i=0;i<question.Options.Length;i++) if (Button(((char)('A'+i)) + " · " + question.Options[i])) answer(i);
        }
        void DrawExam() {
            var q = Level.Exam[QuestionIndex]; bool confirmed = Answers[QuestionIndex] >= 0;
            GUILayout.Label("EXAMEN · Pregunta " + (QuestionIndex+1) + " de " + Level.Exam.Length,title);
            Banner("Score: " + Score + "/" + Level.Exam.Length + " · Selecciona una opción y confirma. Sin tiempo límite.");
            Text(q.Prompt);
            for (int i=0;i<q.Options.Length;i++) {
                GUI.enabled = !confirmed;
                if (Button((selected == i ? "[SELECCIONADA] " : "") + ((char)('A'+i)) + " · " + q.Options[i])) selected = i;
            }
            GUI.enabled = !confirmed && selected >= 0;
            if (Button("Confirmar respuesta")) SubmitAnswer(selected);
            GUI.enabled = true;
            if (confirmed) {
                Banner((Answers[QuestionIndex] == q.Correct ? "CORRECTO · +1 punto. " : "RESPUESTA INCORRECTA · 0 puntos. ") + q.Explanation);
                if (Button(QuestionIndex == Level.Exam.Length-1 ? "Ver resultado final" : "Siguiente pregunta")) NextQuestion();
            }
        }
        void DrawResults() {
            GUILayout.Label("RESULTADO FINAL · " + Level.Title,title);
            Banner("SCORE: " + Score + "/" + Level.Exam.Length + " · " + Mathf.RoundToInt(Score*100f/Level.Exam.Length) + "% de aciertos");
            Text("Examen terminado. Revisa tus respuestas y repite cuando quieras. No hay penalización por practicar.");
            for (int i=0;i<Level.Exam.Length;i++) {
                var q = Level.Exam[i]; GUILayout.Space(12);
                Text((i+1) + ". " + q.Prompt);
                Text("Tu respuesta: " + q.Options[Answers[i]] + (Answers[i] == q.Correct ? " · CORRECTA" : " · INCORRECTA"));
                Text("Respuesta correcta: " + q.Options[q.Correct] + ". " + q.Explanation);
            }
            if (Button("Repetir examen · reiniciar score")) StartExam();
            if (Button("Repasar la clase")) { ResetExam(); feedback = ""; }
            if (Button("Elegir otro nivel")) ReturnToMenu();
        }
        void OnDestroy() {
            if (illustrations != null) foreach (var row in illustrations) foreach (var texture in row) if (texture) Destroy(texture);
            foreach (var texture in new[] {panel,buttonTexture,hoverTexture,accentTexture}) if (texture) Destroy(texture);
        }
    }
}
