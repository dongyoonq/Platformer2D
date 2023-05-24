using BeeState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Bee : MonoBehaviour
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size }

    public TMP_Text text;
    public Transform[] patrolPoints;
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public float traceSpeed;

    private StateBase<Bee>[] states;
    private State currState;

    public Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;

    private void Awake()
    {
        states = new StateBase<Bee>[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);
    }

    private void Start()
    {
        currState = State.Idle;
        text.text = "Idle";
        states[(int)currState].Enter();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        states[(int)currState].Update();
    }

    public void ChangeState(State state)
    {
        switch (state)
        {
            case State.Idle:
                text.text = "Idle";
                break;
            case State.Trace:
                text.text = "Trace";
                break;
            case State.Return:
                text.text = "Return";
                break;
            case State.Attack:
                text.text = "Attack";
                break;
            case State.Patrol:
                text.text = "Patrol";
                break;
        }

        states[(int)currState].Exit();
        currState = state;
        states[(int)currState].Enter();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

namespace BeeState
{
    public class IdleState : StateBase<Bee>
    {
        float idleTime;

        public IdleState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            idleTime = 0f;
            Debug.Log("Idle Enter");
        }

        public override void Exit()
        {
            idleTime = 0f;
            Debug.Log("Idle Exit");
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;

            if (idleTime > 2f)
            {
                idleTime = 0f;
                owner.patrolIndex = (owner.patrolIndex + 1) % owner.patrolPoints.Length;
                owner.ChangeState(Bee.State.Patrol);
            }
            // 플레이어가 가까워졌을 때
            else if (Vector2.Distance(owner.player.position, owner.transform.position) < owner.detectRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }

    public class TraceState : StateBase<Bee>
    {
        public TraceState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("Trace Enter");
        }

        public override void Exit()
        {
            Debug.Log("Trace Exit");
        }

        public override void Update()
        {
            // 플레이어 쫓아가기
            Vector2 TraceDir = (owner.player.position - owner.transform.position).normalized;
            owner.transform.Translate(TraceDir * owner.traceSpeed * Time.deltaTime);

            // 플레이어가 멀어졌을 때
            if (Vector2.Distance(owner.player.position, owner.transform.position) > owner.detectRange)
            {
                owner.ChangeState(Bee.State.Return);
            }
            else if (Vector2.Distance(owner.player.position, owner.transform.position) < owner.attackRange)
            {
                owner.ChangeState(Bee.State.Attack);
            }
        }
    }

    public class ReturnState : StateBase<Bee>
    {
        public ReturnState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("Return Enter");
        }

        public override void Exit()
        {
            Debug.Log("Return Exit");
        }

        public override void Update()
        {
            // 원래 자리로 돌아가기
            Vector2 returnDir = (owner.returnPosition - owner.transform.position).normalized;
            owner.transform.Translate(returnDir * owner.moveSpeed * Time.deltaTime);

            // 원래 자리에 도착했으면
            if (Vector2.Distance(owner.transform.position, owner.returnPosition) < 0.02f)
            {
                owner.ChangeState(Bee.State.Idle);
            }
            // 플레이어가 가까워졌을 때
            if (Vector2.Distance(owner.player.position, owner.transform.position) < owner.detectRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }

    public class AttackState : StateBase<Bee>
    {
        public AttackState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("Attack Enter");
        }

        public override void Exit()
        {
            Debug.Log("Attack Exit");
        }

        public override void Update()
        {
            // 공격하기

            if (Vector2.Distance(owner.player.position, owner.transform.position) > owner.attackRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }

    public class PatrolState : StateBase<Bee>
    {
        public PatrolState(Bee owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("Patrol Enter");
        }

        public override void Exit()
        {
            Debug.Log("Patrol Exit");
        }

        public override void Update()
        {
            // 순찰 위치로 이동
            Vector2 patrolDir = (owner.patrolPoints[owner.patrolIndex].position - owner.transform.position).normalized;
            owner.transform.Translate(patrolDir * owner.moveSpeed * Time.deltaTime);

            // 원래 자리에 도착했으면
            if (Vector2.Distance(owner.transform.position, owner.patrolPoints[owner.patrolIndex].position) < 0.02f)
            {
                owner.ChangeState(Bee.State.Idle);
            }
            // 플레이어가 가까워졌을 때
            if (Vector2.Distance(owner.player.position, owner.transform.position) < owner.detectRange)
            {
                owner.ChangeState(Bee.State.Trace);
            }
        }
    }
}
