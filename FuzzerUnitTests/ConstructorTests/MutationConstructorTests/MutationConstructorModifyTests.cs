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
    internal class MutationConstructorModifyTests
    {
        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Modify_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var mutationConstructor = new MutationConstructor<ConstructClass1>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Modify });
                mutationConstructor.Add(new ConstructClass1()
                {
                    A = "A",
                    B = "B",
                    D = 20,
                    E = 30,
                    F = 40,
                });
                var obj = mutationConstructor.Construct<ConstructClass1>();
                if (obj is not null)
                {
                    Assert.AreEqual(obj.C, 0);
                    var isEquals = new List<bool> { obj.A == "A", obj.B == "B", obj.D == 20, obj.E == 30, obj.F == 40 };
                    var countChanges = isEquals.Count(p => !p);
                    countChanges.Should().BeOneOf(1, 5);
                }
            }
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Modify_OneList_ShouldBeOk()
        {
            var isNullAll = new List<bool>();

            for (int i = 0; i < 5000; i++)
            {
                var mutationConstructor = new MutationConstructor<OneListClass>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetNotNullMainObject();
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Modify });
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

                if (obj != null)
                {
                    if (obj.List != null)
                    {
                        if (obj.List.Count == 4)
                        {
                            if (obj.List.Contains("First") && obj.List.Contains("Second"))
                            {
                                isNullAll.Add(false);
                            }
                        }
                    }
                }

                isNullAll.Add(true);
            }

            isNullAll.All(p => p).Should().BeFalse();
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Modify_OneListWithInnerObjects_ShouldBeOk()
        {
            var isNullAll = new List<bool>();

            for (int i = 0; i < 1000; i++)
            {
                var mutationConstructor = new MutationConstructor<OneListWithInnerObject>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetNotNullMainObject();
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Modify });
                mutationConstructor.Add(new OneListWithInnerObject()
                {
                    List = new List<ConstructClass1>()
                    {
                        new ConstructClass1()
                        {
                            A = "A",
                            D = 20,
                        },
                        new ConstructClass1()
                        {
                            A = "AA",
                            D = 10,
                        }
                    }
                });

                var obj = mutationConstructor.Construct<OneListWithInnerObject>();

                if (obj is not null)
                {
                    if (obj.List is not null)
                    {
                        if (obj.List.Count == 2)
                        {
                            var first = obj.List[0];
                            var second = obj.List[1];

                            if (first is null)
                            {
                                isNullAll.Add(false);
                                continue;
                            }
                            else
                            {
                                if (first.A != "A" || first.D != 20)
                                {
                                    isNullAll.Add(false);
                                    continue;
                                }
                            }

                            if (second is null)
                            {
                                isNullAll.Add(false);
                                continue;
                            }
                            else
                            {
                                if (second.A != "AA" || second.D != 10)
                                {
                                    isNullAll.Add(false);
                                    continue;
                                }
                            }
                        }
                    }
                }

                isNullAll.Add(true);
            }

            isNullAll.All(p => p).Should().BeFalse();
        }

        [Test]
        [Timeout(10000)]
        public void MutationConstructor_Modify_OneDictionary_ShouldBeOk()
        {
            var isNullAll = new List<bool>();
            var deleteLastValue = false;
            for (int i = 0; i < 10000; i++)
            {
                var mutationConstructor = new MutationConstructor<OneDictionary>();
                mutationConstructor.SetCountMutation(1);
                mutationConstructor.SetNotNullMainObject();
                mutationConstructor.SetAllowedMutation(new List<FluentFuzzer.Utils.MutationEnum> { FluentFuzzer.Utils.MutationEnum.Modify });
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
                if (obj is not null)
                {
                    if (obj.Dictionary is not null)
                    {
                        if (obj.Dictionary.Count == 3)
                        {
                            if (!obj.Dictionary.ContainsKey("4") || !obj.Dictionary.ContainsKey("3") || !obj.Dictionary.ContainsKey("2"))
                            {
                                deleteLastValue = true;
                            }

                            isNullAll.Add(false);
                            continue;
                        }

                        if (obj.Dictionary.ContainsValue(null))
                        {
                            isNullAll.Add(false);
                            continue;
                        }

                    }
                }

                isNullAll.Add(true);
            }

            isNullAll.All(p => p).Should().BeFalse();
            deleteLastValue.Should().BeTrue();
        }
    }
}
