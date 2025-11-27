using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    public float slideDuration = 0.5f;

    public Vector3 targetPosition;

    private float timer = 0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool animating = false;
    private System.Action onComplete;

    public void SlideInFrom(Vector3 startPos, float duration = -1f)
    {
        if (duration > 0) slideDuration = duration;

        startPosition = startPos;
        endPosition = targetPosition;
        timer = 0f;
        animating = true;

        onComplete = null;
        transform.position = startPosition;
    }

    public void SlideOutTo(Vector3 endPos, System.Action onFinished = null, float duration = -1f)
    {
        if (duration > 0) slideDuration = duration;

        startPosition = transform.position;
        endPosition = endPos;

        timer = 0f;
        animating = true;

        onComplete = onFinished;
    }

    void Update()
    {
        if (!animating) return;

        timer += Time.deltaTime;
        float t = timer / slideDuration;

        t = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t >= 1f)
        {
            animating = false;
            onComplete?.Invoke();
        }
    }
}
