string piNumber = "3,";
        int dividedBy = 11080585;
        int divisor = 78256779;
        int result;

        for (int i = 0; i < digitNumber; i++)
        {
            if (dividedBy < divisor)
                dividedBy *= 10;

            result = dividedBy / divisor;

            string resultString = result.ToString();
            piNumber += resultString;

            dividedBy = dividedBy - divisor * result;
        }

        return piNumber;
        
}
