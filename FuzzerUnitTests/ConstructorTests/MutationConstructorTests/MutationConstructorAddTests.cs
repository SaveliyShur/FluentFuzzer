using FluentAssertions;
using FluentFuzzer.Constructors.Constructors;
using FuzzerRunner;
using FuzzerUnitTests.ConstructorTests.ConstructClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzerUnitTests.ConstructorTests.MutationConstructorTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MutationConstructorAddTests
    {
        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Add_String_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var mutationConstructor = new MutationConstructor<ConstructClass1>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Add });
                mutationConstructor.Add(new ConstructClass1()
                {
                    B = "B",
                    D = 20,
                    E = 30,
                    F = 40,
                });
                var obj = mutationConstructor.Construct<ConstructClass1>();
                if (obj is not null)
                {
                    Assert.AreEqual(obj.C, 0);
                    var isEquals = new List<bool> { obj.B == "B", obj.D == 20, obj.E == 30, obj.F == 40 };
                    isEquals.Where(i => !i).ToList().Should().HaveCount(0);
                }
            }
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Add_Int_ShouldBeOk()
        {
            var intValues = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                var mutationConstructor = new MutationConstructor<ConstructClass1>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Add });
                mutationConstructor.Add(new ConstructClass1()
                {
                    A = "A",
                    B = "B",
                    D = 20,
                    E = 30,
                });
                var obj = mutationConstructor.Construct<ConstructClass1>();
                if (obj is not null)
                {
                    Assert.AreEqual(obj.C, 0);
                    var isEquals = new List<bool> { obj.A == "A", obj.B == "B", obj.D == 20, obj.E == 30 };
                    isEquals.Where(i => !i).ToList().Should().HaveCount(0);
                    intValues.Add(obj.F);
                }
            }

            intValues.Any(i => i != 0).Should().BeTrue();
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Add_OneList_ShouldBeOk()
        {
            var isFive = false;

            for (int i = 0; i < 1000; i++)
            {
                var mutationConstructor = new MutationConstructor<OneListClass>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetNotNullMainObject();
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Add });
                mutationConstructor.Add(new OneListClass()
                {
                    List = new List<string>()
                    {
                        "First",
                        "Second",
                        "Third",
                        "Fourth",
                    }
                });

                var obj = mutationConstructor.Construct<OneListClass>();

                obj.List.Count.Should().BeOneOf(4, 5);

                if (obj.List.Count == 5)
                    isFive = true;
            }

            isFive.Should().BeTrue();
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Add_OneListWithInnerObjects_ShouldBeOk()
        {
            var isSetB = false;

            for (int i = 0; i < 5000; i++)
            {
                var mutationConstructor = new MutationConstructor<OneListWithInnerObject>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetNotNullMainObject();
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Add });
                mutationConstructor.Add(new OneListWithInnerObject()
                {
                    List = new List<ConstructClass1>()
                    {
                        new ConstructClass1()
                        {
                            A = "A",
                            D = 20,
                            E = 30,
                            F = 40,
                        },
                    }
                });

                var obj = mutationConstructor.Construct<OneListWithInnerObject>();

                if (obj.List.Count == 1)
                {
                    if (obj.List.First().B != null && obj.List.First().B != string.Empty)
                    {
                        isSetB = true;
                    }
                }
                else
                {
                    obj.List.Count.Should().Be(2);
                }
            }

            isSetB.Should().BeTrue();
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor__Add_OneDictionary_ShouldBeOk()
        {
            var haveCountFive = false;
            for (int i = 0; i < 10000; i++)
            {
                var mutationConstructor = new MutationConstructor<OneDictionary>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetNotNullMainObject();
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Add });
                mutationConstructor.Add(new OneDictionary()
                {
                    Dictionary = new Dictionary<string, string>()
                    {
                        { "1", "1" },
                        { "2", "2" },
                        { "3", "3" },
                        { "4", "4" },
                    }
                });

                var obj = mutationConstructor.Construct<OneDictionary>();

                if (obj.Dictionary.Count == 5)
                {
                    haveCountFive = true;
                }

                obj.Dictionary["1"].Should().Be("1");
                obj.Dictionary["2"].Should().Be("2");
                obj.Dictionary["3"].Should().Be("3");
                obj.Dictionary["4"].Should().Be("4");
            }

            haveCountFive.Should().BeTrue();
        }
    }
}
