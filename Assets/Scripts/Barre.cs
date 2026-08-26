using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Barre : NetworkBehaviour
{
    [SerializeField] private float vitesse = 15f;
    [SerializeField] private float limite = 12f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Autorité Owner : seul le propriétaire a le droit d'écrire.
        if (!IsOwner) return;

        // IsOwnedByServer == true pour la barre de l'hôte, false pour celle du client.
        float x = IsOwnedByServer ? -20f : 20f;
        transform.position = new Vector3(x, 0.5f, 0f);
    }

    void Update()
    {
        if (!IsOwner) return;      // équivalent ici à IsLocalPlayer
        GestionDeplacement();
    }

    void GestionDeplacement()
    {
        if (Keyboard.current == null) return;

        float direction = 0f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            direction = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            direction = -1f;

        if (Mathf.Approximately(direction, 0f)) return;

        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z + direction * vitesse * Time.deltaTime, -limite, limite);
        transform.position = pos;
    }
}