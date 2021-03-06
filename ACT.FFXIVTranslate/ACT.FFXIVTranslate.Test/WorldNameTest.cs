﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACT.FFXIVTranslate.Test
{
    [TestClass]
    public class WorldNameTest
    {
        [TestMethod]
        public void TestParseSenderName()
        {
                        var bytes = new byte[]
                        {
                            0x02, 0x27, 0x13, 0x01, 0x0F, 0x01, 0x01, 0xEF, 0xBF, 0xBD, 0x20, 0x4D, 0x69, 0x6C, 0x61, 0x20,
                            0x45, 0x74, 0x65, 0x72, 0x6E, 0x61, 0x6C, 0x03, 0xEE, 0x82, 0x96, 0x4D, 0x69, 0x6C, 0x61, 0x20,
                            0x45, 0x74, 0x65, 0x72, 0x6E, 0x61, 0x6C, 0x02, 0x27, 0x07, 0xEF, 0xBF, 0xBD, 0x01, 0x01, 0x01,
                            0xEF, 0xBF, 0xBD, 0x01, 0x03, 0x02, 0x12, 0x02, 0x59, 0x03, 0x55, 0x6C, 0x74, 0x69, 0x6D, 0x61
                        };
//            var bytes = new byte[]
//            {
//                0x02, 0x12, 0x02, 0x59, 0x03, 0x55, 0x6C, 0x74, 0x69, 0x6D, 0x61
//            };

            var input = Encoding.UTF8.GetString(bytes);

            var v = TextProcessor.ExtractName(input);
        }
    }
}
