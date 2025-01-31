﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ChatTest.App.Services
{
    public class UserNameGenerator : IUserNameGenerator
    {
        private static readonly ConcurrentStack<string> Usernames;
        // ReSharper disable once InconsistentNaming
        private static readonly IReadOnlyCollection<string> _all;

        public IEnumerable<string> All => _all;

        static UserNameGenerator()
        {
            _all = new List<string>
                   {
                       "guldurgoal",
                       "ridiculousindian",
                       "cockatoourge",
                       "worstvictorious",
                       "inactivemerciful",
                       "freshsolemn",
                       "indicationdusty",
                       "arguedense",
                       "stridentscoreboard",
                       "smifincarcarous",
                       "introducelowly",
                       "testhawser",
                       "strangeboatswain",
                       "rememberbarronneau",
                       "knitvisit",
                       "benefitmalkin",
                       "hijackorgan",
                       "exerciseadmonish",
                       "pitcheredible",
                       "slothelated",
                       "aldermandunlin",
                       "mischiefexclaim",
                       "automaticdug",
                       "fwoopercelebrant",
                       "defargesquat",
                       "heronafford",
                       "agreeamusing",
                       "equableptarmigan",
                       "jauntyinspiring",
                       "drowntalkative",
                       "carcgazingi",
                       "impolitethundering",
                       "pricklyconfidence",
                       "simplisticmetacarpus",
                       "slumkeyanalyze",
                       "miggsrealize",
                       "taxidriverrita",
                       "servantbarman",
                       "blueeyedwinning",
                       "uncommonsuburban",
                       "livelymew",
                       "lardsiege",
                       "wannucleus",
                       "performdisaster",
                       "osgiliathcoati",
                       "staggbangham",
                       "aboardjaunty",
                       "wakeimpala",
                       "flusteredfive",
                       "decorplummer",
                       "sloppycareer",
                       "lieethiopian"
                   };

            Usernames = new ConcurrentStack<string>(_all);
        }

        public string Generate() => Usernames.TryPop(out var name) ? name : throw new InvalidOperationException("No usernames left");
        
    }
}
