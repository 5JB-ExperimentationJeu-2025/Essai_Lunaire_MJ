using UnityEngine;

public class AvatarIKSync : MonoBehaviour
{
    public Transform teteCam;
    public Transform mainGaucheController;
    public Transform mainDroiteController;

    public Transform teteAvatar;
    public Transform mainGaucheAvatar;
    public Transform mainDroiteAvatar;

    public Transform corps;
    public Vector3 cou;

    void LateUpdate()
    {
        if (teteCam && teteAvatar)
        {
            teteAvatar.position = teteCam.position;
            teteAvatar.rotation = teteCam.rotation;
            corps.position = teteAvatar.position + cou;
        }

        if (mainGaucheController && mainGaucheAvatar)
        {
            mainGaucheAvatar.position = mainGaucheController.position;
            mainGaucheAvatar.rotation = mainGaucheController.rotation;
        }

        if (mainDroiteController && mainDroiteAvatar)
        {
            mainDroiteAvatar.position = mainDroiteController.position;
            mainDroiteAvatar.rotation = mainDroiteController.rotation;
        }
    }
}
