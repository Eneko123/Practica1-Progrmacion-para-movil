using System.Collections.Generic;
using TMPro;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UI;

public class UsuariosUI : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI contentText;

    [Header("Botones")]
    // Referencias de botones
    public Button newUsuariAddButton;
    public Button addButton;
    public Button nullRertyButton;
    public Button errorRertyButton;
    public Button readdNewUsuariButton;
    public Button showListButton;

    [Header("Paneles")]
    public GameObject initialPanel;
    public GameObject addPanel;
    // Referencias paneles emergentes
    public GameObject nullDataPanel;
    public GameObject errorPanel;
    public GameObject successPanel;
    public GameObject listPanel;

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
    private float SearchID = 0;

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
        readdNewUsuariButton.onClick.AddListener(() =>
        {
            addPanel.SetActive(true);
            successPanel.SetActive(false);
        });
        showListButton.onClick.AddListener(ShowList);
    }

    // Se esperan strings desde los InputField
    public void ReadName(string Newname)
    {
        userName = Newname;
        Debug.Log($"Nombre ingresado: {userName}");
    }

    public void ReadAge(string Newage)
    {
        ageText = Newage;
        int.TryParse(Newage, out ageInt);
        Debug.Log($"Edad ingresada: {ageText}");
    }

    public void ReadID(string NewID)
    {
        float.TryParse(NewID, out float ID);
        SearchID = ID;
        Debug.Log($"ID ingresado: {ID}");
    }

    // Funcion para agregar un usuario a la lista
    void AddUsuario()
    {
        // Validar que los campos no estén vacios y que la edad sea un numero valido
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(ageText))
        {
            nullDataPanel.SetActive(true);
            return;
        }
        else if (ageInt < 0 || ageInt > 120)
        {
            errorPanel.SetActive(true);
            return;
        }

        Usuario usuario = new Usuario(usuariosList.Count + 1, userName, ageInt);

        usuariosList.Add(usuario);

        addPanel.SetActive(false);
        successPanel.SetActive(true);

        Debug.Log($"Usuario agregado: ID={usuario.ID}, Name={usuario.name}, Age={usuario.age}");
    }

    void Rerty()
    {
        nullDataPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    void ShowList()
    {
        initialPanel.SetActive(false);
        listPanel.SetActive(true);

        for (int i = 0; i < usuariosList.Count; i++)
        {
            contentText.text = $"{usuariosList[i].ID}, {usuariosList[i].name}, {usuariosList[i].age}\n";
        }
    }

    void ShowUsuariForID()
    {
        for (int i = 0; i < usuariosList.Count; i++)
        {
            if (usuariosList[i].ID == SearchID)
            {
                contentText.text = $"{usuariosList[i].ID}, {usuariosList[i].name}, {usuariosList[i].age}\n";
            }
            else
            {
                contentText.text = $"No se encontro un usuario con ID {SearchID}\n";
            }
        }
    }

    void ShowOldest()
    {
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
                contentText.text += $"{usuariosList[i].ID}, {usuariosList[i].name}, {usuariosList[i].age}\n";
            }
        }
    }

    void DeleteUsuari()
    {
        for (int i = 0; i < usuariosList.Count; i++)
        {
            if (usuariosList[i].ID == SearchID)
            {
                usuariosList.RemoveAt(i);
            }
            else
            {
                contentText.text = $"No se encontro un usuario con ID {SearchID}\n";
            }
        }
    }
}
