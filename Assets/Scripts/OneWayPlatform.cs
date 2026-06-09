using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneWayPlatform : MonoBehaviour
{
    private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        float platformTop = _col.bounds.max.y;
        float playerBottom = collision.collider.bounds.min.y;

        // player vindo de baixo: ignora colisão e agenda reativação
        if (playerBottom < platformTop - 0.05f)
        {
            Physics.IgnoreCollision(collision.collider, _col, true);
            StartCoroutine(ReenableCollision(collision.collider));
        }
    }

    private System.Collections.IEnumerator ReenableCollision(Collider playerCol)
    {
        // espera o player passar completamente pela plataforma
        yield return new WaitForSeconds(0.3f);
        if (playerCol != null && _col != null)
            Physics.IgnoreCollision(playerCol, _col, false);
    }
}