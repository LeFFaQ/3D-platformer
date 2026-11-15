using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    [Header("Health parameters")]
    [SerializeField] private int m_health = 5;
    
    
    public void Hit(int damage)
    {
        Debug.Log($"Object {gameObject.name} hit with {damage} damage ");
        m_health -= damage;

        if (m_health == 0)
        {
            Debug.Log($"Object {gameObject.name} dead ");
            //Destroy(gameObject);
        }  
    } 

    public void Heal(int hp) { 
        Debug.Log($"Object {gameObject.name} healed with {hp} points ");
        m_health += hp;
       
    }

    public int GetHealth() => m_health;
}
