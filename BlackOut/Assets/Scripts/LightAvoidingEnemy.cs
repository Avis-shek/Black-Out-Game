using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAvoidingEnemy : MonoBehaviour
{
    public Transform player;
    public Light2D playerLight; // Reference to player's light (using Universal Render Pipeline)
    public float moveSpeed = 2f;
    public float safeDistanceBuffer = 0.5f; // How much farther than light radius enemy keeps away

    private void Update()
    {
        if (player == null || playerLight == null) return;

        Vector2 enemyPos = transform.position;
        Vector2 playerPos = player.position;
        float lightRadius = playerLight.pointLightOuterRadius;

        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(enemyPos, playerPos);

        float minDistance = lightRadius + safeDistanceBuffer;
        float step = moveSpeed * Time.deltaTime;

        Vector2 direction = (enemyPos - playerPos).normalized; // move AWAY if too close

        if (distanceToPlayer < minDistance)
        {
            // Enemy is inside the light � retreat!
            transform.position = Vector2.MoveTowards(enemyPos, enemyPos + direction, step);
        }
        else if (distanceToPlayer > minDistance + 0.5f)
        {
            // Enemy is too far � creep back in a bit
            direction = (playerPos - enemyPos).normalized;
            transform.position = Vector2.MoveTowards(enemyPos, enemyPos + direction, step);
        }
        // Else: enemy is hovering just at the light boundary � do nothing
    }

}
