using System;
using System.IO;
using AulaInclusiva;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class ClassroomProject
{
    static ClassroomProject() { EditorApplication.playModeStateChanged += OnPlay; }
    const string ScenePath = "Assets/Scenes/AulaInclusiva.unity";
    static Material Material(string name, Color color) {
        string path = "Assets/Materials/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (!m) { m = new Material(Shader.Find("Standard")); AssetDatabase.CreateAsset(m, path); }
        m.color = color; return m;
    }
    static GameObject Box(string name, Vector3 position, Vector3 size, Material material) {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name; obj.transform.position = position; obj.transform.localScale = size;
        obj.GetComponent<Renderer>().sharedMaterial = material; return obj;
    }
    static void Furniture(string model, Vector3 position, Material material) {
        var source = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/" + model + ".obj");
        if (!source) throw new Exception("Missing model: " + model);
        var obj = (GameObject)PrefabUtility.InstantiatePrefab(source);
        obj.name = model; obj.transform.position = position;
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>()) renderer.sharedMaterial = material;
        var collider = obj.AddComponent<BoxCollider>();
        collider.center = new Vector3(0, model == "Mesa" ? .42f : .55f, 0);
        collider.size = model == "Mesa" ? new Vector3(1.4f,.84f,.8f) : new Vector3(.52f,1.1f,.52f);
    }
    [MenuItem("Aula Inclusiva/Regenerar escena (reemplaza la escena del prototipo)")]
    public static void Generate() {
        AssetDatabase.Refresh();
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var floor = Material("Piso", new Color(.24f,.30f,.36f));
        var wall = Material("Pared", new Color(.86f,.88f,.83f));
        var wood = Material("Madera", new Color(.65f,.39f,.17f));
        var chair = Material("Sillas", new Color(.12f,.40f,.48f));
        var boardMaterial = Material("Pizarra", new Color(.025f,.12f,.13f));
        Box("Piso", new Vector3(0,-.15f,0), new Vector3(12,.3f,14), floor);
        Box("Pared frontal", new Vector3(0,2,7), new Vector3(12,4,.2f), wall);
        Box("Pared posterior", new Vector3(0,2,-7), new Vector3(12,4,.2f), wall);
        Box("Pared izquierda", new Vector3(-6,2,0), new Vector3(.2f,4,14), wall);
        Box("Pared derecha", new Vector3(6,2,0), new Vector3(.2f,4,14), wall);
        Box("Marco pizarra", new Vector3(0,2.05f,6.78f), new Vector3(5.4f,2.2f,.12f), wood);
        var board = Box("Pizarra interactiva", new Vector3(0,2.05f,6.65f), new Vector3(5.1f,1.95f,.12f), boardMaterial);
        board.AddComponent<Blackboard>();
        var sign = new GameObject("Texto de pizarra");
        sign.transform.position = new Vector3(0,2.3f,6.56f); sign.transform.rotation = Quaternion.identity;
        var text = sign.AddComponent<TextMesh>(); text.text = "AULA INCLUSIVA\n\nAcércate y presiona E";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        sign.GetComponent<MeshRenderer>().sharedMaterial = text.font.material;
        text.fontSize = 64; text.characterSize = .07f; text.anchor = TextAnchor.MiddleCenter; text.alignment = TextAlignment.Center;
        for (int row = 0; row < 3; row++) foreach (float x in new[] { -3f,3f }) {
            Furniture("Mesa", new Vector3(x,0,2-row*2.8f), wood);
            Furniture("Silla", new Vector3(x,0,.95f-row*2.8f), chair);
        }
        var light = new GameObject("Luz principal").AddComponent<Light>();
        light.type = LightType.Directional; light.intensity = 1.1f; light.transform.rotation = Quaternion.Euler(50,-30,0);
        RenderSettings.ambientLight = new Color(.65f,.65f,.65f);
        var player = new GameObject("Estudiante"); player.transform.position = new Vector3(0,.05f,-4.8f);
        var body = player.AddComponent<CharacterController>(); body.height = 1.8f; body.radius = .3f; body.center = new Vector3(0,.9f,0); body.stepOffset = .25f;
        var movement = player.AddComponent<FirstPersonController>();
        var camera = new GameObject("Camara").AddComponent<Camera>(); camera.tag = "MainCamera";
        camera.transform.SetParent(player.transform); camera.transform.localPosition = new Vector3(0,1.65f,0); camera.nearClipPlane = .05f;
        camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = new Color(.18f,.24f,.29f);
        movement.view = camera;
        var lesson = new GameObject("Leccion visual").AddComponent<ClassroomLesson>(); lesson.player = movement;
        PlayerSettings.productName = "Aula Inclusiva 3D"; PlayerSettings.companyName = "Grupo 7";
        PlayerSettings.defaultScreenWidth = 1280; PlayerSettings.defaultScreenHeight = 800;
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        var settings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        settings.FindProperty("activeInputHandler").intValue = 0; settings.ApplyModifiedPropertiesWithoutUndo();
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath,true) };
        AssetDatabase.SaveAssets();
        Debug.Log("AULA_GENERATED: scene, models and build settings saved.");
    }
    public static void PreviewAndBuild() {
        Generate();
        var camera = UnityEngine.Object.FindFirstObjectByType<Camera>();
        var render = new RenderTexture(1280,800,24);
        camera.targetTexture = render; camera.Render(); RenderTexture.active = render;
        var image = new Texture2D(1280,800,TextureFormat.RGB24,false);
        image.ReadPixels(new Rect(0,0,1280,800),0,0); image.Apply();
        Directory.CreateDirectory("Documentation"); File.WriteAllBytes("Documentation/Aula.png", image.EncodeToPNG());
        camera.targetTexture = null; RenderTexture.active = null;
        UnityEngine.Object.DestroyImmediate(image); render.Release(); UnityEngine.Object.DestroyImmediate(render);
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { ScenePath }, locationPathName = "Builds/AulaInclusiva3D/AulaInclusiva3D.exe",
            target = BuildTarget.StandaloneWindows64, options = BuildOptions.None
        });
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            throw new Exception("Windows build failed: " + report.summary.result);
        Debug.Log("AULA_WINDOWS_BUILD_PASSED");
    }
    public static void SmokeTest() {
        EditorSceneManager.OpenScene(ScenePath);
        SessionState.SetBool("AulaSmoke", true);
        EditorApplication.isPlaying = true;
    }
    static double start;
    static void OnPlay(PlayModeStateChange state) {
        if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool("AulaSmoke", false)) return;
        start = EditorApplication.timeSinceStartup;
        EditorApplication.update += Check;
    }
    static void Require(bool condition, string message) { if (!condition) throw new Exception(message); Debug.Log("PASS: " + message); }
    static void Check() {
        if (EditorApplication.timeSinceStartup - start < 2) return;
        EditorApplication.update -= Check;
        try {
            var lesson = UnityEngine.Object.FindFirstObjectByType<ClassroomLesson>();
            var player = lesson.player; var body = player.GetComponent<CharacterController>();
            Require(UnityEngine.Object.FindObjectsByType<Blackboard>(FindObjectsSortMode.None).Length == 1, "One interactive board");
            Require(!lesson.CanInteract(), "Board inaccessible from starting position");
            var initial = player.transform.position; player.Move(Vector2.up,.1f);
            Require(player.transform.position.z > initial.z, "Forward movement");
            body.enabled = false; player.transform.position = new Vector3(0,.05f,4.5f); body.enabled = true;
            Physics.SyncTransforms();
            Require(lesson.CanInteract(), "Raycast reaches board at reading distance");
            lesson.Interact(); Require(lesson.Open && player.Reading, "Interaction opens lesson and pauses navigation");
            lesson.Next(); lesson.Next(); Require(lesson.Page == 2, "Lesson navigation");
            lesson.Answer(false); Require(!lesson.Completed, "Incorrect answer permits retry");
            lesson.Answer(true); Require(lesson.Completed, "Correct answer completes lesson");
            lesson.Close(); Require(!lesson.Open && !player.Reading, "Closing restores navigation");
            body.enabled = false; player.transform.position = new Vector3(0,.05f,6.1f); body.enabled = true;
            Physics.SyncTransforms(); for (int i=0;i<30;i++) player.Move(Vector2.up,.05f);
            Require(player.transform.position.z < 6.6f, "Board collider blocks movement");
            SessionState.SetBool("AulaSmoke", false);
            Debug.Log("AULA_SMOKE_TEST_PASSED"); EditorApplication.Exit(0);
        } catch (Exception exception) { Debug.LogException(exception); EditorApplication.Exit(1); }
    }
}



