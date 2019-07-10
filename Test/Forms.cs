#nullable enable

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using EZHTTP;
using System.Collections.Generic;

[TestClass()]
public class Test {
    private class TestForm : IEquatable<TestForm> {
        public string Checkbox { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

        public static bool operator ==(TestForm obj, TestForm that) {
            if (obj is null || that is null) return obj is null && that is null;
            return obj.Checkbox == that.Checkbox
                && obj.Text == that.Text
                && obj.Color == that.Color;
        }

        public static bool operator !=(TestForm obj, TestForm that) {
            return !(obj == that);
        }

        public override bool Equals(object that) {
            return that is TestForm testForm && testForm == this;
        }

        public bool Equals(TestForm that) {
            return that == this;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

    private struct TestFormStruct : IEquatable<TestFormStruct> {
        public string Checkbox { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

        public static bool operator ==(TestFormStruct obj, TestFormStruct that) {
            return obj.Checkbox == that.Checkbox
                && obj.Text == that.Text
                && obj.Color == that.Color;
        }

        public static bool operator !=(TestFormStruct obj, TestFormStruct that) {
            return !(obj == that);
        }

        public override bool Equals(object that) {
            return that is TestFormStruct testForm && testForm == this;
        }

        public bool Equals(TestFormStruct that) {
            return that == this;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

    private enum Choice {
        Yes,
        No,
        Maybe
    }

    private struct EnumForm : IEquatable<EnumForm> {
        public Choice choice;

        public override bool Equals(object obj) {
            return obj is EnumForm form && Equals(form);
        }

        public bool Equals(EnumForm other) {
            return choice == other.choice;
        }

        public override int GetHashCode() {
            return 790427672 + choice.GetHashCode();
        }

        public static bool operator ==(EnumForm left, EnumForm right) {
            return left.Equals(right);
        }

        public static bool operator !=(EnumForm left, EnumForm right) {
            return !(left == right);
        }
    }

    [TestMethod()]
    public void Equality() {
        Assert.IsFalse(new TestForm() == null);
        Assert.IsFalse(null == new TestForm());
        Assert.IsTrue((TestForm)null == (TestForm)null);
    }

    [TestMethod()]
    public void EmptyForm() {
        Assert.IsTrue(Forms.Parse<TestForm>("") == new TestForm());
        Console.ReadKey();
        Assert.IsTrue(true);
    }

    [TestMethod()]
    public void ParseForm() {
        string s = "checkbox=on&text=Hello+World&color=#880000";
        Assert.IsTrue(Forms.Parse<TestForm>(s) == new TestForm {
            Checkbox = "on",
            Text = "Hello World",
            Color = "#880000"
        });
    }
    
    [TestMethod()] 
    public void ParseFormStruct() {
        string s = "checkbox=on&text=Hello+World&color=#880000";
        Assert.IsTrue(Forms.Parse<TestFormStruct>(s) == new TestFormStruct {
            Checkbox = "on",
            Text = "Hello World",
            Color = "#880000"
        });
    }

    [TestMethod()]
    public void ParseColor() {
        Assert.IsTrue(Color.Parse("#77ff33") == new Color (
            Red: 0x77,
            Green: 0xff,
            Blue: 0x33
        ));
        Assert.ThrowsException<ParseException>(() => Color.Parse("77ff33"));
    }

    [TestMethod()]
    public void ParseEnum() {
        Assert.IsTrue(Forms.Parse<EnumForm>("choice=maybe") == new EnumForm {
            choice = Choice.Maybe
        });
    }
}