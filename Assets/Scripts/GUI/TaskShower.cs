
using AIAD.Exceptions;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace AIAD
{
    public sealed class TaskShower : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TaskText;

        [SerializeField] private float TaskCompleteShowingDelay;
        [SerializeField] private float TransitionToShowingDelay;
        [SerializeField] private float PrevTaskRemoveSymInterval;
        [SerializeField] private float NextTaskShowingSymInterval;

        [SerializeField] private Color NormalColor;
        [SerializeField] private Color TaskCompleteColor;

        private string CurrentTask="";

        private void Awake()
        {
            string ExcSrc = "TaskShower.Awake()";

            if (TaskText == null)
                throw new AIADException("Missing TaskText.", ExcSrc);
            if (TaskCompleteShowingDelay < 0)
                throw new AIADException("TaskCompleteShowingDelay must be equal or greater than zero.", ExcSrc);
            if (PrevTaskRemoveSymInterval <= 0)
                throw new AIADException("PrevTaskRemoveSpeed must be greater than zero.", ExcSrc);
            if (NextTaskShowingSymInterval <= 0)
                throw new AIADException("NextTaskShowingSpeed must be greater than zero.", ExcSrc);

            CurrentTask = TaskText.text;
            Registry.TaskShower = this;
        }

        private IEnumerator ChangeTask(string oldTask,string newTask)
        {
            StringBuilder currText = new StringBuilder(oldTask);
            if (currText.Length > 0)
            {
                TaskText.text = currText.ToString();
                TaskText.color = TaskCompleteColor;
                yield return new WaitForSeconds(TaskCompleteShowingDelay);
                while (currText.Length > 0)
                {
                    currText.Remove(currText.Length - 1, 1);
                    TaskText.text = currText.ToString();
                    yield return new WaitForSeconds(PrevTaskRemoveSymInterval);
                }
                yield return new WaitForSeconds(TransitionToShowingDelay);
            }
            TaskText.color = NormalColor;
            int i = 0;
            while (currText.Length < newTask.Length)
            {
                currText.Append(newTask[i++]);
                TaskText.text = currText.ToString();
                yield return new WaitForSeconds(NextTaskShowingSymInterval);
            }
        }
        public void SetNewTask(string task)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTask(CurrentTask, task));
            CurrentTask = task;
        }
    }
}
