

    interface IInitInMain
    {
        public void Init();
    }

    interface IEnterInStart
    {
        public void Start();
    }

    interface IExitInGame
    {
        public void Stop();
    }

    interface IEnterInUpdate
    {
        void Update(float timeDelta);
    }

    interface IEnterInPhysicUpdate
    {
        void PhysicUpdate(float timeDelta);
    }
    
