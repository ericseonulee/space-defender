using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

public class PathFinderSingle : MonoBehaviour {
    EnemySpawnerSingle enemySpawnerSingle;
    List<Transform> waypoints;
    Player player;
    Health health;

    int waypointIndex = 0;
    int followPlayerCount = 2;
    float horizontalRandomOffset;
    float verticalRandomOffset;

    void Awake() {
        player = GameObject.Find("Player").GetComponent<Player>();
        horizontalRandomOffset = Random.Range(-0.5f, 0.5f);
        verticalRandomOffset = Random.Range(-2f, 2f);
        health = gameObject.GetComponent<Health>();

        if (player == null) {
            Debug.Log("Cannot find Player.");
        }

        if (health == null) {
            Debug.Log("Cannot find Health");
        }
    }

    void Start() {
        StartCoroutine(FollowPath());
    }

    void Update() {
        if (health.IsDead()) {
            StopAllCoroutines();
        }
    }

    IEnumerator FollowPath() {
        Shooter shooter = this.GetComponentInChildren<Shooter>();

        while (waypointIndex < waypoints.Count) {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            float delta = enemySpawnerSingle.GetCurrentWave().GetMoveSpeed() * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, delta);

            if (transform.position == targetPosition) {
                waypointIndex++;

                if (waypointIndex >= 1 && Random.Range(0, 3) > 3) {
                    shooter.EnemyShootOnce();
                    yield return new WaitForSeconds(1f);
                }
                else {
                    yield return new WaitForSeconds(0.5f);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        Vector3 playerPosition = new Vector3(player.transform.position.x + horizontalRandomOffset,
                                        transform.position.y + verticalRandomOffset,
                                        transform.position.z);

        while (transform.position != playerPosition) {
            float delta = enemySpawnerSingle.GetCurrentWave().GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, delta);

            if (transform.position == playerPosition) {
                shooter.EnemyShootOnce();
                yield return new WaitForSeconds(1.75f);
            }
            yield return new WaitForEndOfFrame();
        }

        Vector3 closestPoint = transform.position * 2 + GetClosestBorder();

        while (transform.position != closestPoint) {    
            float delta = enemySpawnerSingle.GetCurrentWave().GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, closestPoint, delta);

            if (transform.position == closestPoint) {
                Destroy(gameObject);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetEnemySpawner(EnemySpawnerSingle enemySpawnerSingle) {
        this.enemySpawnerSingle = enemySpawnerSingle;
    }

    public void SetWayPoints(List<Transform> waypoints) {
        this.waypoints = waypoints;
        transform.position = this.waypoints[waypointIndex].position;
    }

    Vector3 GetClosestBorder() {
        Vector3 position = gameObject.transform.position;
        float leftBorderX = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        float RightBorderX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;
        float topBorderY = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
        float bottomBorderY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        Dictionary<string, float> borders = new Dictionary<string, float>();

        borders.Add("leftDelta", leftBorderX - position.x);
        borders.Add("rightDelta", RightBorderX - position.x);
        borders.Add("topDelta", topBorderY - position.y);
        borders.Add("bottomDelta", bottomBorderY - position.y);

        KeyValuePair<string, float> result = borders.Aggregate((p1, p2) => (Mathf.Abs(p1.Value) < Mathf.Abs(p2.Value)) ? p1 : p2);

        if (result.Key == "leftDelta" || result.Key == "rightDelta") {
            return new Vector3(result.Value, 0, 0);
        }
        return new Vector3(0, result.Value, 0);
    }
}
