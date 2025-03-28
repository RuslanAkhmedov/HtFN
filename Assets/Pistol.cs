using UnityEngine;

public class PistolSlide : MonoBehaviour
{
    public Transform slide; // Ссылка на объект slide
    public float slideBackDistance = 0.1f; // Расстояние для сдвига затвора
    private Vector3 initialPosition; // Исходная позиция затвора

    void Start()
    {
        // Запоминаем начальную позицию затвора
        initialPosition = slide.localPosition; // Используем локальную позицию
    }

    void Update()
    {
        // Если нажата кнопка для выстрела
        if (Input.GetButtonDown("Fire1")) // Используйте любую кнопку для выстрела
        {
            AudioManager.Instance?.PlayShotSound();
            // Сдвигаем затвор назад по оси Z
            slide.localPosition = initialPosition - new Vector3(0, 0, slideBackDistance); // Используем localPosition
            Invoke("ReturnSlide", 0.1f);
        }
    }

    // Метод для возврата затвора в начальное положение
    public void ReturnSlide()
    {
        // Возвращаем затвор в начальное положение
        slide.localPosition = initialPosition; // Используем localPosition
    }
}
