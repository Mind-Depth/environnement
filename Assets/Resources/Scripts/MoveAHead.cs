using UnityEngine;
using Valve.VR;

public class MoveAHead : MonoBehaviour
{
    private Vector3 save;
    float speedXZ;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        float x = 0, z = 0;
        OpenVR.Chaperone.GetPlayAreaSize(ref x, ref z);
        speedXZ =
            (x <= 1.95f) ? 30f :
            (x <= 2.05f) ? 25f :
            (x <= 2.25f) ? 20f :
            (x <= 2.35f) ? 19f :
            (x <= 2.45f) ? 14.5f :
            9.2f;
        save = Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position != save) {
            transform.position = new Vector3(
                (save.x - transform.position.x) * speedXZ,
                (save.y - transform.position.y) * 1,
                (save.z - transform.position.z) * speedXZ
            );
            save = Player.position;
        }
    }
}
