using System.Collections; 
using UnityEngine; 
using UnityEngine.UI;

public class Tweening : MonoBehaviour {

    //private static Vector3 damp = default(Vector3);
    //public static IEnumerator SetScale(Transform tf, Vector3 to, float spd = .75f) {
    //    Vector3 damp = default;
    //    if (to == Vector3.one) tf.gameObject.SetActive(true);
    //    while (tf.localScale != to) {
    //        tf.localScale = Vector3.SmoothDamp(tf.localScale, to, ref damp, spd * Time.unscaledDeltaTime);
    //        yield return null;
    //    }
    //    if (to == Vector3.zero) tf.gameObject.SetActive(false);
    //    yield break;    
    //}

    public static IEnumerator SetScale(Transform tf, Vector3 to, float spd = .75f)
    {
        Vector3 damp = default;
        
        while (tf.localScale != to)
        {
            tf.localScale = Vector3.SmoothDamp(tf.localScale, to, ref damp, spd * Time.unscaledDeltaTime);
            yield return null;
        }
        
        yield break;
    }


    //public static IEnumerator SetScaledPosition(Transform tf, Vector3 pos, Vector3 scale, float sSpeed = 16, float pSpeed = 850, bool local = false) {

    //    Debug.Log("HaciendoTwening SetScaledPosition");

    //     if (scale == Vector3.one) tf.gameObject.SetActive(true);

    //    if (!local) {
    //        while (tf.localScale != scale || tf.position != pos) {
    //            tf.localScale = Vector3.MoveTowards(tf.localScale, scale, sSpeed * Time.unscaledDeltaTime);
    //            tf.position = Vector3.MoveTowards(tf.position, pos, pSpeed * Time.unscaledDeltaTime);

    //            yield return null;
    //        }
    //    }
    //    else {
    //        while (tf.localScale != scale || tf.localPosition != pos) {
    //            tf.localScale = Vector3.MoveTowards(tf.localScale, scale, sSpeed * Time.unscaledDeltaTime);
    //            tf.localPosition = Vector3.MoveTowards(tf.localPosition, pos, pSpeed * Time.unscaledDeltaTime);

    //            yield return null;
    //        }
    //    }

    //     if (scale == Vector3.zero) tf.gameObject.SetActive(false);

    //    yield break;
    //}

    public static IEnumerator SetScaledPosition(Transform tf, Vector3 pos, Vector3 scale, float sSpeed = 16f, float pSpeed = 850f, bool local = false)
    {       

        if (!local)
        {
            while (Vector3.Distance(tf.localScale, scale) > 0.01f || Vector3.Distance(tf.position, pos) > 0.01f)
            {
                tf.localScale = Vector3.MoveTowards(tf.localScale, scale, sSpeed * Time.unscaledDeltaTime);
                tf.position = Vector3.MoveTowards(tf.position, pos, pSpeed * Time.unscaledDeltaTime);
                yield return null;
            }
            tf.localScale = scale;
            tf.position = pos;
        }
        else
        {
            while (Vector3.Distance(tf.localScale, scale) > 0.01f || Vector3.Distance(tf.localPosition, pos) > 0.01f)
            {
                tf.localScale = Vector3.MoveTowards(tf.localScale, scale, sSpeed * Time.unscaledDeltaTime);
                tf.localPosition = Vector3.MoveTowards(tf.localPosition, pos, pSpeed * Time.unscaledDeltaTime);
                yield return null;
            }
            tf.localScale = scale;
            tf.localPosition = pos;
        }
    }


    public static IEnumerator SetPosition(Transform tf, Vector3 pos, bool enable, float speed = 1, bool local = false) {
        Vector3 damp = Vector3.zero;
        if (enable) tf.gameObject.SetActive(enable);
        if (!local) {
            while (tf.position != pos) {
                tf.position = Vector3.SmoothDamp(tf.position, pos, ref damp, speed * Time.unscaledDeltaTime);
                yield return null;
            }
        }
        else {
            while (tf.localPosition != pos) {
                tf.localPosition = Vector3.SmoothDamp(tf.localPosition, pos, ref damp, speed * Time.unscaledDeltaTime);
                yield return null;
            }
        }
        if (!enable) tf.gameObject.SetActive(enable);
        yield break;
    }
    public static IEnumerator SetFloat(float or, float to, float speed) {
        float damp = 0;
        while (or != to) {
            or = Mathf.SmoothDamp(or, to, ref damp, speed * Time.unscaledDeltaTime);
            yield return null;
        }
    }
    public static IEnumerator SetFillAmount(Image img, float to, bool active, float speed = 1f) {
        float damp = 0;
        if (active) img.gameObject.SetActive(true);
        while (img.fillAmount != to) {
            img.fillAmount = Mathf.SmoothDamp(img.fillAmount, to, ref damp, speed * Time.unscaledDeltaTime);
            yield return null;
        }
        if (!active) img.gameObject.SetActive(false);
        yield break;
    }
    public static IEnumerator SetAlpha(Image sprite, float to, float speed) {
        float damp = 0;
        var tempColor = sprite.color;
        if (to > 0) sprite.gameObject.SetActive(true);
        while (tempColor.a != to && sprite.gameObject.activeSelf) {
            tempColor.a = Mathf.SmoothDamp(tempColor.a, to, ref damp, speed * Time.unscaledDeltaTime); ;
            sprite.color = tempColor;
            yield return null;
        }
        if (to <= 0) sprite.gameObject.SetActive(false);
        yield break;
    }
}
