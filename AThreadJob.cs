using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classe abstratraite de laquelle héritent les différents threads.
 * Permet de lister les tâches threadées, de les lancer facilement et d'avoir une harmonisation de celles-ci.
 */ 
public abstract class AThreadJob {

    private bool m_IsDone = false;
    private object m_Handle = new object();
    private System.Threading.Thread m_Thread = null;
    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (m_Handle)
            {
                tmp = m_IsDone;
            }
            return tmp;
        }
        set
        {
            lock (m_Handle)
            {
                m_IsDone = value;
            }
        }
    }

    /* Lance le thread.
     * Dès que le thread est prêt il appelle la méthode "Run".
     */
    public virtual void Start()
    {
        m_Thread = new System.Threading.Thread(Run);
        m_Thread.Start();
    }
    public virtual void Abort() {
        m_Thread.Abort();
    }

    /*
     * Fonction qui va être la tâche du thread en question.
     * Une fois que cette fonction est finie, elle passe l'attribut "IsDone" a "True".
     */
    protected abstract void ThreadFunction();

    protected abstract void OnFinished();

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }
    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }
    private void Run()
    {
        try
        {
            ThreadFunction();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        IsDone = true;
    }
}
