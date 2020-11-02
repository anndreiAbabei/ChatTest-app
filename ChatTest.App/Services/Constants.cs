using System;

namespace ChatTest.App.Services
{
    public static class Constants
    {
        public const string PrivateKey = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDbLR3xWWn2NFOXhp78OvQZ5Q3jOI8omdsRxIJsygw4dLBOpWxpkAglmwoRmClhhOr2bl1iX1oOskU9zvkZJLSgou2Hzy5Wlza6xnP6WfP4y93tSjwU2JFRyfQVl3Hia8x0Av1wbSe0GBhaRD6bo1J9PrTYy7qC9wV82zKvsr06/6nA8a4X2CkhPBpXdFC4LLIdRo+pz1guHEZWZcFd+fcOd++0Tsk3U5Rsh4rVKthfxT0G3/uHP1XMeH366Pa0GmB8ITewF6PNXLhVVjzYv2kPR5WNI1NonPS1QTUzSap2LHTRXTufzNqo/XdtlEF1rQDe6KvCdyOrWBuGDMIQx8hTAgMBAAECggEAGX2Hy/gAMNgpGKhZeTBKD3DqSpDbZXdvWQOnv4tIHJDFqH/oueY8bFM5uo4d0e+pe/ud9MDuMmNdVjDqG5h/QWdxhKf91ahEfzhxaJ5pl3fuWZiWoD4HuKFqN9CjOBgdcSif3SF0yqc6vdTOCz/VaieO1N/s0ypzgu/jVzfMdj3qmVByqmhMs1CrvZmKWHDTNxB+4QWHU+5/+nNkryhhGw2PEVskYuu//rg6u5GPv1bIyX2jZethjN7F6v4/tperBW815faM712VWhcsOg186bkQYbxFnGXndZNVE68pmWiFhTkCrM6kjDGBdVJQAIIoLGxDqbYzNnj4xgYNuFUHAQKBgQDlG8u2lyjLe/XPeindj5WAeXgPUyxn53mbuymGcscxAxRTeqVoaTyCqLdIdoWf0dR1mQgA77g/qqDkO3NUbL0XLLyvSXZnHw/iO3By6NiViq3TtnmZEQy2903Mg+NhWaXcsJ5WQYurjJpJYiF1v05mTxHY2py9fubgk90+M9I4kwKBgQD05uBgEFZ1Gi2r2sMpziKXaYJlJjC7KNDimPjzbViEO6o0vyEXP1dBx0yMq8BQo8EUzeNeXC/YZBCa1J0bxKoleCjWaun0zrpOSU4IO8ArNN2qovv4hcugtOxEPiLd8yR9MyIuunCynAQj8NrwKQe5k6dDBRQSDJh8NKj5Z5TJQQKBgAS8pWKaD3rdjkCC6xisnk+wsz3F33YqgAYrQXmJJ/socCQltgPJoTmmWVvDL5IVWYow5sx2KF4QnhD59bF6KhKvlxscrrkFGGP6DKIjlE7LNjrZW/xBMP0bcd1XoLzjiJ1efXeVFVkvqAT6ZWy0zt8opVrRckostINSK1Hc6mzHAoGBAIbPgLvgwnb7ziBH8B7XJhuLLQhZGYItWa8i3gfCLXO9FFiszZ5qc7H176GP+fFp8yNsoriSrpPDoZBZcZKTk8XEe1ZszBTCp7Pojlfh7Erccxk3VtTbcdgpP9XhDnh3G6L/vtVSNC9dyrmuIBcLvwDc0roF1aKJ5Haa1llUN4cBAoGBAKhQxEAWbYActBDnWkUnp808SdfZH6x+Wbq/qv3gwi442ZDAxn4EkEEEYZjtiNrDtxSARmm/Xwpgb8g2uGH6DPF2r6gom7iZUOTVfUIHvb6FoltiY3El1UTTxDnkVIzyCFaZnA92+dOoq/rfZw3HFcXEzbDSoJhOmx73ltSmCqI4";


        internal static class Mock
        {
            public const bool UseMock = true;

            public static readonly Guid[] ConversationIds =
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            public static readonly string[] UserNames =
            {
                "lieethiopian",
                "sloppycareer",
                "decorplummer",
                "flusteredfive",
            };
        }
    }
}
