 
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
    

    Animator _animator;
    public Transform target;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimWalk(bool walk)
    {
            _animator.SetBool("Walk",walk);
    }

    public void Moved()
    {AnimWalk(false);
        /*transform.DOMove(target.position, 4).OnComplete((() =>
        {
            AnimWalk(false);
        }));*/
        
    }
}
