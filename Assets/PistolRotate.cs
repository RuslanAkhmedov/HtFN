using UnityEngine;

public class PistolAim : MonoBehaviour
{
    public Vector3 positionOffset = new Vector3(0.5f, -0.3f, 0.6f);
    public Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

    void Update()
    {
        // ����� ��������� �������
        transform.localPosition = positionOffset;

        // ����� ��������� ������� 
        transform.localRotation = Quaternion.Euler(rotationOffset);
    }
}