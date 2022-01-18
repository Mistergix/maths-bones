using UnityEngine;

namespace PGSauce.Save
{
    public abstract class SavedData<T> : SavedDataBase
    {
        [SerializeField] private T defaultValue;
        public string Key => name;
        public T DefaultValue => defaultValue;

        public void SaveData(T value)
        {
            PgSave.Instance.Save(this, value);
        }
        
        public T Load()
        {
            return PgSave.Instance.Load(this);
        }
    }
}