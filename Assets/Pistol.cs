using UnityEngine;

public class PistolSlide : MonoBehaviour
{
    public Transform slide; // ������ �� ������ slide
    public float slideBackDistance = 0.1f; // ���������� ��� ������ �������
    private Vector3 initialPosition; // �������� ������� �������

    void Start()
    {
        // ���������� ��������� ������� �������
        initialPosition = slide.localPosition; // ���������� ��������� �������
    }

    void Update()
    {
        // ���� ������ ������ ��� ��������
        if (Input.GetButtonDown("Fire1")) // ����������� ����� ������ ��� ��������
        {
            AudioManager.Instance?.PlayShotSound();
            // �������� ������ ����� �� ��� Z
            slide.localPosition = initialPosition - new Vector3(0, 0, slideBackDistance); // ���������� localPosition
            Invoke("ReturnSlide", 0.1f);
        }
    }

    // ����� ��� �������� ������� � ��������� ���������
    public void ReturnSlide()
    {
        // ���������� ������ � ��������� ���������
        slide.localPosition = initialPosition; // ���������� localPosition
    }
}
