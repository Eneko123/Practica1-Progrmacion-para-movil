using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsuariosUI : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI contentText;

    [Header("Botones")]
    public Button newUsuariAddButton;
    public Button addButton;
    public Button nullRertyButton;
    public Button errorRertyButton;
    public Button reAddNewUsuariButton;
    public Button showListButton;
    public Button reShowListButton;
    public Button reAddNewUsuariFromListPanelButton;
    public Button showListOldestButton;
    public Button searchByIDButton;
    public Button deleteByIDButton;
    public Button confirmSearchIDButton;
    public Button confirmDeleteIDButton;

    [Header("Paneles")]
    public GameObject initialPanel;
    public GameObject addPanel;
    public GameObject nullDataPanel;
    public GameObject errorPanel;
    public GameObject successPanel;
    public GameObject listPanel;
    public GameObject seachIDPanel;
    public GameObject dleteIDPanel;

    [Header("Input Fields")]
    public TMP_InputField nameInputField;
    public TMP_InputField ageInputField;
    public TMP_InputField searchIDInputField;
    public TMP_InputField deleteIDInputField;

    [Header("Notificaciones temporales")]
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public float notificationDuration = 2f;

    [Header("Borrado con retardo")]
    public GameObject deleteProgressPanel;
    public Slider deleteProgressSlider;
    public TextMeshProUGUI deleteCountdownText;
    public float deleteDelaySeconds = 3f;

    // Datos del ususario
    public class Usuario
    {
        public float ID;
        public string name;
        public int age;
        public Usuario(float ID, string name, int age)
        {
            this.ID = ID;
            this.name = name;
            this.age = age;
        }
    }
    // Lista de usuarios
    public List<Usuario> usuariosList = new List<Usuario>();

    // Variables para almacenar la entrada del usuario
    private string userName = string.Empty;
    private string ageText = string.Empty;
    private int ageInt = 0;
    private bool ageIsValid = false;
    private float SearchID = 0;
    private string searchIDText = string.Empty;
    private string deleteIDText = string.Empty;

    // Contador de IDs
    private float nextID = 1f;

    private Coroutine notificationCoroutine;
    private Coroutine deleteCoroutine;

    void Start()
    {
        newUsuariAddButton.onClick.AddListener(() =>
        {
            initialPanel.SetActive(false);
            addPanel.SetActive(true);
        });
        addButton.onClick.AddListener(AddUsuario);
        nullRertyButton.onClick.AddListener(Rerty);
        errorRertyButton.onClick.AddListener(Rerty);
        reAddNewUsuariButton.onClick.AddListener(() =>
        {
            addPanel.SetActive(true);
            successPanel.SetActive(false);
        });
        showListButton.onClick.AddListener(ShowList);
        showListButton.onClick.AddListener(() =>
        {
            successPanel.SetActive(false);
            listPanel.SetActive(true);
        });
        reShowListButton.onClick.AddListener(ShowList);
        reAddNewUsuariFromListPanelButton.onClick.AddListener(() =>
        {
            addPanel.SetActive(true);
            listPanel.SetActive(false);
        });
        showListOldestButton.onClick.AddListener(ShowOldest);
        searchByIDButton.onClick.AddListener(() =>
        {
            listPanel.SetActive(false);
            seachIDPanel.SetActive(true);
        });
        deleteByIDButton.onClick.AddListener(() =>
        {
            listPanel.SetActive(false);
            dleteIDPanel.SetActive(true);
        });
        confirmSearchIDButton.onClick.AddListener(ConfirmSearchByID);
        confirmDeleteIDButton.onClick.AddListener(ConfirmDeleteByID);
        // Sirve para limitar los inputs a numeros y que no ponga una edad exageradamente alta
        ageInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        ageInputField.characterLimit = 3;
    }

    // Funciones para leer lo datos introducidos por el ususario
    public void ReadName(string Newname)
    {
        userName = Newname;
    }

    public void ReadAge(string Newage)
    {
        ageText = Newage;
        ageIsValid = int.TryParse(Newage, out ageInt);
    }

    public void ReadIDForSearchByID(string NewID)
    {
        searchIDText = NewID;
    }

    public void ReadIDForDeleteByID(string NewID)
    {
        deleteIDText = NewID;
    }

    // Funciones para confirmar la accion del usuario
    public void ConfirmSearchByID()
    {
        if (string.IsNullOrWhiteSpace(searchIDText) || !float.TryParse(searchIDText, out float ID))
        {
            ShowNotification("Introduce un ID válido");
            return;
        }

        SearchID = ID;
        ShowUsuariForID();
    }

    public void ConfirmDeleteByID()
    {
        if (string.IsNullOrWhiteSpace(deleteIDText) || !float.TryParse(deleteIDText, out float ID))
        {
            ShowNotification("Introduce un ID válido");
            return;
        }

        SearchID = ID;
        DeleteUsuari();
    }

    // Funcion para agregar un usuario a la lista
    void AddUsuario()
    {
        // Validar que los campos no esten vacios y que la edad sea un numero valido
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(ageText) || !ageIsValid)
        {
            nullDataPanel.SetActive(true);
            return;
        }
        else if (ageInt < 0 || ageInt > 120)
        {
            errorPanel.SetActive(true);
            return;
        }

        Usuario usuario = new Usuario(nextID, userName, ageInt);
        nextID++;
        usuariosList.Add(usuario);

        addPanel.SetActive(false);
        successPanel.SetActive(true);

        CleanAreas();

        ShowNotification($"Usuario '{usuario.name}' añadido correctamente");
    }

    // Funcion para limpiar los campos que el usuario a introducido y no quede raro
    void CleanAreas()
    {
        userName = string.Empty;
        ageText = string.Empty;
        ageInt = 0;
        ageIsValid = false;
        nameInputField.text = string.Empty;
        ageInputField.text = string.Empty;
    }

    void Rerty()
    {
        nullDataPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    void ShowList()
    {
        // Medida de seguridad aunque es imposible mostrar la lista sin minimo haber añadido un usuario
        if (usuariosList.Count == 0)
        {
            contentText.text = "No hay usuarios registrados.";
            return;
        }

        contentText.text = "";
        for (int i = 0; i < usuariosList.Count; i++)
        {
            // Imprimr los usuarios
            contentText.text += $"{usuariosList[i].ID},     {usuariosList[i].name},     {usuariosList[i].age}\n";
        }
    }

    void ShowUsuariForID()
    {
        listPanel.SetActive(true);
        seachIDPanel.SetActive(false);
        searchIDInputField.text = string.Empty;

        // Buscamos el indice una sola vez
        int index = usuariosList.FindIndex(u => u.ID == SearchID);

        if (index != -1)
        { 
            Usuario u = usuariosList[index];
            contentText.text = $"{u.ID}, {u.name}, {u.age}\n";
        }
        else
        {
            contentText.text = $"No se encontró un usuario con ID {SearchID}\n";
            ShowNotification($"No se encontró un usuario con ID {SearchID}");
        }
    }

    void ShowOldest()
    {
        if (usuariosList.Count == 0)
        {
            contentText.text = "No hay usuarios registrados.";
            return;
        }

        // Buscar la edad maxima
        int maxAge = 0;
        for (int i = 0; i < usuariosList.Count; i++)
        {
            if (usuariosList[i].age > maxAge)
            {
                maxAge = usuariosList[i].age;
            }
        }

        // Mostrar todos los que tengan esa edad
        contentText.text = "";
        for (int i = 0; i < usuariosList.Count; i++)
        {
            if (usuariosList[i].age == maxAge)
            {
                contentText.text += $"{usuariosList[i].ID},     {usuariosList[i].name},     {usuariosList[i].age}\n";
            }
        }
    }

    // Busca el usuario y, si existe, lanza el borrado con retardo de 3s
    void DeleteUsuari()
    {
        dleteIDPanel.SetActive(false);
        deleteIDInputField.text = string.Empty;

        int index = usuariosList.FindIndex(u => u.ID == SearchID);

        if (index == -1)
        {
            listPanel.SetActive(true);
            ShowNotification($"No se encontró un usuario con ID {SearchID}");
            return;
        }

        // Cuenta atras / barra de progreso antes de borrar
        if (deleteCoroutine != null) StopCoroutine(deleteCoroutine);
        deleteCoroutine = StartCoroutine(DeleteUsuariRoutine(index));
    }

    IEnumerator DeleteUsuariRoutine(int index)
    {
        deleteProgressPanel.SetActive(true);
        deleteProgressSlider.minValue = 0f;
        deleteProgressSlider.maxValue = 1f;
        deleteProgressSlider.value = 0f;

        float elapsed = 0f;
        while (elapsed < deleteDelaySeconds)
        {
            elapsed += Time.deltaTime;
            float remaining = Mathf.Max(deleteDelaySeconds - elapsed, 0f);

            if (deleteProgressSlider != null)
                deleteProgressSlider.value = elapsed / deleteDelaySeconds;

            if (deleteCountdownText != null)
                deleteCountdownText.text = Mathf.CeilToInt(remaining).ToString();

            yield return null;
        }

        // Comprovacion de seguridad por si el indice cambia mientras borramos el usuario
        if (index >= 0 && index < usuariosList.Count)
        {
            Usuario usuario = usuariosList[index];
            usuariosList.RemoveAt(index);
            ShowNotification($"Usuario '{usuario.name}' eliminado correctamente");
        }

        if (deleteProgressPanel != null) deleteProgressPanel.SetActive(false);

        listPanel.SetActive(true);
        ShowList();

        deleteCoroutine = null;
    }

    // Notificacion temporal reutilizable
    public void ShowNotification(string message, float duration = -1f)
    {
        if (duration < 0f) duration = notificationDuration;

        if (notificationCoroutine != null) StopCoroutine(notificationCoroutine);
        notificationCoroutine = StartCoroutine(NotificationRoutine(message, duration));
    }

    IEnumerator NotificationRoutine(string message, float duration)
    {
        if (notificationText != null) notificationText.text = message;
        if (notificationPanel != null) notificationPanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (notificationPanel != null) notificationPanel.SetActive(false);
        notificationCoroutine = null;
    }
}