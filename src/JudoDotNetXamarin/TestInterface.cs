using System;

namespace JudoDotNetXamarin
{
    public interface TestInterface
    {
        void Payment (PaymentViewModel payment, JudoSuccessCallback success, JudoFailureCallback failure);
    }
}

