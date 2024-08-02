using UnityEngine;

public static class UnityGameObjectExtensions
{
    public static GameObject? GetFirstChildObjectWithTag(this GameObject parent, string childTag)
    {
        int childCount = parent.transform.childCount;

        if (string.IsNullOrWhiteSpace(childTag))
        {
            return parent.transform.GetChild(0).gameObject ?? null;
        }

        if (childCount != 0)
        {
            for (int childIndex = 0; childIndex < childCount; childIndex++)
            {
                var childTransform = parent.transform.GetChild(childIndex);

                if (childTransform.tag == childTag)
                {
                    return childTransform.gameObject;
                }
            }
        }

        return null;
    }
}