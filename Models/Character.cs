using System.Collections.Generic;

namespace Sheltered2SaveGameEditor.Models;

internal sealed partial class Character
{
    internal string FirstName { get; set; } = string.Empty;
    internal string LastName { get; set; } = string.Empty;
    internal int CurrentHealth { get; set; }
    internal int MaxHealth { get; set; }
    internal bool Interacting { get; set; }
    internal bool InteractingWithObj { get; set; }
    internal bool HasBeenDefibbed { get; set; }
    internal bool PassedOut { get; set; }
    internal bool IsUnconscious { get; set; }

    // Stats
    internal Stat Strength { get; set; } = new Stat();
    internal Stat Dexterity { get; set; } = new Stat();
    internal Stat Intelligence { get; set; } = new Stat();
    internal Stat Charisma { get; set; } = new Stat();
    internal Stat Perception { get; set; } = new Stat();
    internal Stat Fortitude { get; set; } = new Stat();

    // Skills lists (can be expanded as needed)
    internal List<SkillInstance> StrengthSkills { get; set; } = [];

    internal string FullName => $"{FirstName} {LastName}".Trim();
}

internal class Stat
{
    internal int Level { get; set; } = 1;

    internal int Cap => Level <= 5 ? Level * 2 : 20;
}

internal class SkillInstance
{
    internal SkillDefinition Definition { get; }
    internal int Level { get; set; }

    internal SkillInstance(SkillDefinition definition, int level)
    {
        Definition = definition;
        Level = level;
    }
}

internal class SkillDefinition
{
    internal int Key { get; }
    internal string Name { get; }
    internal string Description { get; }
    internal SkillDefinition(int key, string name, string description)
    {
        Key = key;
        Name = name;
        Description = description;
    }
}