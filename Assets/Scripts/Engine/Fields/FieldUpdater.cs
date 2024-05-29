using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Place at top of hierarchy to handle field updates automatically
/// </summary>
public class FieldUpdater : MonoBehaviour
{
    private static  List<FieldSource> sources;
    private static List<Element> targets;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        foreach (FieldSource source in sources) 
        {
            var job = new UpdateElementsWithFieldSource()
            {
                source = source
            };
            job.Schedule(targets.Count, 1);
        }
    }
    /// <summary>
    /// Must be called when adding a field source
    /// </summary>
    /// <param name="source"></param>
    public static void AddFieldSource(FieldSource source)
    {
        sources.Add(source);
    }
    /// <summary>
    /// Must be called when spawning an element than can be affected by fields
    /// </summary>
    /// <param name="element"></param>
    public static void AddFieldTarget(Element element) 
    {
        targets.Add(element);
    }

    struct UpdateElementsWithFieldSource : IJobParallelFor
    {
        public FieldSource source;
        public void Execute(int i)
        {
            if (source.field.ActsOn(targets[i]))
                source.field.Affect(targets[i]);
        }
    }

}
