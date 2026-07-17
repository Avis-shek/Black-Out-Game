using UnityEngine;

public class Battery : MonoBehaviour
{
    public float lightBoost = 2f;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("⚡ Battery picked up!");

            GameManager.Instance.AddLight(lightBoost);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
