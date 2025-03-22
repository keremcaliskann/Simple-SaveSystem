# Simple-PoolManager

Simple Pool Manager for Unity

# How To Use

- Add PoolManager.cs to your scene
- Adjust your pool list in inspector or you can create pool via scripts. Add PoolManager.Instance.CreatePool(prefab, 10, "Pool Item")
- Call PoolManager.Instance.GetPoolObject("Pool Item")