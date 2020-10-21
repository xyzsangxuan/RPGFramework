using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public float _x;
    public float _y;
    public float _z;
    public float _speed;

    //public Entity ballEntity;
    //private EntityManager manager;

    public Transform player;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {/*
        if (ballEntity == null) { return; }
        Translation ballPos = manager.GetComponentData<Translation>(ballEntity);
        _x = Mathf.Clamp(_x, -60, 60);
        Vector3 _pos = transform.position;

        _pos.x = ballPos.Value.x - _x;
        _pos.y = ballPos.Value.y - _y;
        _pos.z = ballPos.Value.z - _z;
        transform.position = Vector3.Lerp(transform.position, _pos, Time.deltaTime * _speed);
        transform.LookAt(ballPos.Value);
*/
        Vector3 tartgetPosition =player.position;
        _x = Mathf.Clamp(_x, -60, 60);
        Vector3 _pos = transform.position;
        _pos.x = tartgetPosition.x - _x;
        _pos.y = tartgetPosition.y - _y;
        _pos.z = tartgetPosition.z - _z;
        transform.position = Vector3.Lerp(transform.position, _pos, Time.deltaTime * _speed);
        //transform.LookAt(tartgetPosition);
    }

    public void Rotate(float horizontal)
    {
        _x += horizontal;
    }
}

