using TMPro;
using UnityEngine;




public class Indice : MonoBehaviour
{
    

    public TextMeshProUGUI trace;
    public TextMeshProUGUI indication;
    public TMP_InputField nomJoueur;


    public void Valider()
    {
        if (indication.text != "MERCI") {
            trace.text = nomJoueur.text;
            nomJoueur.text= string.Empty;
            indication.text = "MERCI";
        }
        else {  
            trace.text = transform.position.ToString();
        }

    }
}
