using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationModel
{
    public class Simulation
    {
        private List<Person>[,] maleSubjects;
        private List<Person>[,] femaleSubjects;
        private List<Person>[,] bornedSubjects;
        private List<Person>[,] deceasedSubjects;
        private int year_count;
        private int month_count;
        private Dictionary<string, Person> usedIds;
        
        public Simulation()
        {
            CalcM();
            SetSeed();

            year_count = 1;
            month_count = 1;
            usedIds = new Dictionary<string, Person>();
            maleSubjects = new List<Person>[101, 13];
            femaleSubjects = new List<Person>[101, 13];
            bornedSubjects = new List<Person>[101, 13];
            deceasedSubjects = new List<Person>[101, 13];

            for(int i = 1; i < 101; ++i)
            {
                for(int j = 1; j < 13; ++j)
                {
                    maleSubjects[i, j] = new List<Person>();
                    femaleSubjects[i, j] = new List<Person>();
                    bornedSubjects[i, j] = new List<Person>();
                    deceasedSubjects[i, j] = new List<Person>();
                }
            }

            for (int i = 0; i < 50; ++i)
            {
                string id;
                int age;
                int desired_children;
                int birth_month;

                id = Generate_Id();
                age = Generate_Uniform(0, 100);
                desired_children = Generate_Desired_Children();
                birth_month = Generate_Uniform(1, 12);
                maleSubjects[1, 1].Add(new Person(id, Gender.Male, birth_month, desired_children));

                id = Generate_Id();
                age = Generate_Uniform(0, 100);
                desired_children = Generate_Desired_Children();
                birth_month = Generate_Uniform(1, 12);
                femaleSubjects[1, 1].Add(new Person(id, Gender.Female, birth_month, desired_children));
            }

            while(year_count < 100)
            {
                List<Person> subjects = new List<Person>();
                foreach(Person p in maleSubjects[year_count, month_count])
                {
                    subjects.Add(p);
                }

                foreach(Person p in femaleSubjects[year_count, month_count])
                {
                    subjects.Add(p);
                }

                foreach(Person p in subjects)
                {
                    if (p.birthday_month == month_count)
                    {
                        p.Increase_Age();
                    }

                    if (p.lonelyness_time > 0)
                    {
                        p.Decrease_Lonelyness_Time();
                    }

                    double uniform_variable_2 = Generate_Uniform();
                    if (p.age >= 12)
                    {
                        if (p.age <= 15)
                        {
                            if (uniform_variable_2 < 0.6)
                            {
                                p.Set_Mating_Desire(true);
                            }
                            else
                            {
                                p.Set_Mating_Desire(false);
                            }
                        }
                        else
                        {
                            if (p.age <= 21)
                            {
                                if (uniform_variable_2 < 0.65)
                                {
                                    p.Set_Mating_Desire(true);
                                }
                                else
                                {
                                    p.Set_Mating_Desire(false);
                                }
                            }
                            else
                            {
                                if (p.age <= 35)
                                {
                                    if (uniform_variable_2 < 0.8)
                                    {
                                        p.Set_Mating_Desire(true);
                                    }
                                    else
                                    {
                                        p.Set_Mating_Desire(false);
                                    }
                                }
                                else
                                {
                                    if (p.age <= 45)
                                    {
                                        if (uniform_variable_2 < 0.6)
                                        {
                                            p.Set_Mating_Desire(true);
                                        }
                                        else
                                        {
                                            p.Set_Mating_Desire(false);
                                        }
                                    }
                                    else
                                    {
                                        if (p.age <= 60)
                                        {
                                            if (uniform_variable_2 < 0.5)
                                            {
                                                p.Set_Mating_Desire(true);
                                            }
                                            else
                                            {
                                                p.Set_Mating_Desire(false);
                                            }
                                        }
                                        else
                                        {
                                            if (p.age <= 125)
                                            {
                                                if (uniform_variable_2 < 0.2)
                                                {
                                                    p.Set_Mating_Desire(true);
                                                }
                                                else
                                                {
                                                    p.Set_Mating_Desire(false);
                                                }
                                            }
                                            else
                                            {
                                                if (uniform_variable_2 < 0.05)
                                                {
                                                    p.Set_Mating_Desire(true);
                                                }
                                                else
                                                {
                                                    p.Set_Mating_Desire(false);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach(Person p in subjects)
                {
                    if(p.gender == Gender.Female)
                    {
                        if (p.pregnancy_months_left > 0)
                        {
                            p.Decrease_Pregnancy_Months_Left();
                            if (p.pregnancy_months_left == 0)
                            {
                                for (int i = 0; i < p.number_of_babyes; ++i)
                                {
                                    string id;
                                    Gender gender;
                                    int desired_children = -1;
                                    double uniform_variable;

                                    id = Generate_Id();

                                    // baby gender
                                    uniform_variable = Generate_Uniform();
                                    if (uniform_variable < 0.5)
                                    {
                                        gender = Gender.Male;
                                    }
                                    else
                                    {
                                        gender = Gender.Female;
                                    }

                                    // number of desired children of baby
                                    desired_children = Generate_Desired_Children();

                                    Person baby = new Person(id, gender, month_count, desired_children);
                                    bornedSubjects[year_count, month_count].Add(baby);
                                    subjects.Add(baby);
                                }
                                p.Increase_Children_Count(p.number_of_babyes);
                                p.Set_Number_Of_Babyes(-1);
                                p.Set_Pregnancy_Month_Left(-1);
                                try
                                {
                                    Person father = usedIds[p.father_id];

                                    if(father != null && father.alive)
                                    {
                                        father.Increase_Children_Count(p.number_of_babyes);
                                    }
                                }
                                catch
                                {
                                    throw new Exception("Some error ocurred -> Father of the kid/s not founded");
                                }

                            }
                        }
                    }

                    double uniform_variable_2 = Generate_Uniform();
                    if(p.age <= 12)
                    {
                        if(p.gender == Gender.Male)
                        {
                            if(uniform_variable_2 < 0.25)
                            {
                                Decease(p, year_count, month_count);
                                continue;
                            }
                        }
                        else
                        {
                            if (uniform_variable_2 < 0.25)
                            {
                                Decease(p, year_count, month_count);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (p.age <= 45)
                        {
                            if (p.gender == Gender.Male)
                            {
                                if (uniform_variable_2 < 0.1)
                                {
                                    Decease(p, year_count, month_count);
                                    continue;
                                }
                            }
                            else
                            {
                                if (uniform_variable_2 < 0.15)
                                {
                                    Decease(p, year_count, month_count);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (p.age <= 76)
                            {
                                if (p.gender == Gender.Male)
                                {
                                    if (uniform_variable_2 < 0.3)
                                    {
                                        Decease(p, year_count, month_count);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (uniform_variable_2 < 0.35)
                                    {
                                        Decease(p, year_count, month_count);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                if (p.age <= 125)
                                {
                                    if (p.gender == Gender.Male)
                                    {
                                        if (uniform_variable_2 < 0.7)
                                        {
                                            Decease(p, year_count, month_count);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (uniform_variable_2 < 0.65)
                                        {
                                            Decease(p, year_count, month_count);
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    if (p.gender == Gender.Male)
                                    {
                                        if (uniform_variable_2 < 0.99)
                                        {
                                            Decease(p, year_count, month_count);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (uniform_variable_2 < 0.98)
                                        {
                                            Decease(p, year_count, month_count);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if(p.mate_id != "")
                    {
                        uniform_variable_2 = Generate_Uniform();
                        if(uniform_variable_2 < 0.2)
                        {
                            try
                            {
                                Person exMate = usedIds[p.mate_id];

                                p.Set_Mate_ID("");
                                exMate.Set_Mate_ID("");
                                p.Set_Lonelyness_Time(Generate_Lonelyness_Time(p.age));
                                exMate.Set_Lonelyness_Time(Generate_Lonelyness_Time(exMate.age));
                            }
                            catch
                            {
                                if(p.gender == Gender.Male)
                                    throw new Exception("Some error ocurred -> Imaginary girlfriend");
                                else
                                    throw new Exception("Some error ocurred -> Imaginary boyfriend");
                            }
                        }
                    }

                    if(p.gender == Gender.Female && p.mate_id != "" && p.pregnancy_months_left != -1)
                    {
                        try
                        {
                            Person mate = usedIds[p.mate_id];

                            if((p.children_count < p.desired_children || p.desired_children == -1) && (mate.children_count < mate.desired_children || mate.desired_children == -1))
                            {
                                uniform_variable_2 = Generate_Uniform();
                                if (p.age <= 15)
                                {
                                    if (uniform_variable_2 < 0.2)
                                    {
                                        p.Set_Pregnancy_Month_Left(9);
                                        p.Set_Father_ID(mate.id);
                                        double uniform_variable_3 = Generate_Uniform();
                                        if(uniform_variable_3 < 0.7)
                                        {
                                            p.Set_Number_Of_Babyes(1);
                                        }
                                        else
                                        {
                                            if (uniform_variable_3 < 0.88)
                                            {
                                                p.Set_Number_Of_Babyes(2);
                                            }
                                            else
                                            {
                                                if (uniform_variable_3 < 0.95)
                                                {
                                                    p.Set_Number_Of_Babyes(3);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.98)
                                                    {
                                                        p.Set_Number_Of_Babyes(4);
                                                    }
                                                    else
                                                    {
                                                        p.Set_Number_Of_Babyes(5);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (p.age <= 21)
                                    {
                                        if (uniform_variable_2 < 0.45)
                                        {
                                            p.Set_Pregnancy_Month_Left(9);
                                            p.Set_Father_ID(mate.id);
                                            double uniform_variable_3 = Generate_Uniform();
                                            if (uniform_variable_3 < 0.7)
                                            {
                                                p.Set_Number_Of_Babyes(1);
                                            }
                                            else
                                            {
                                                if (uniform_variable_3 < 0.88)
                                                {
                                                    p.Set_Number_Of_Babyes(2);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.95)
                                                    {
                                                        p.Set_Number_Of_Babyes(3);
                                                    }
                                                    else
                                                    {
                                                        if (uniform_variable_3 < 0.98)
                                                        {
                                                            p.Set_Number_Of_Babyes(4);
                                                        }
                                                        else
                                                        {
                                                            p.Set_Number_Of_Babyes(5);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (p.age <= 35)
                                        {
                                            if (uniform_variable_2 < 0.8)
                                            {
                                                p.Set_Pregnancy_Month_Left(9);
                                                p.Set_Father_ID(mate.id);
                                                double uniform_variable_3 = Generate_Uniform();
                                                if (uniform_variable_3 < 0.7)
                                                {
                                                    p.Set_Number_Of_Babyes(1);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.88)
                                                    {
                                                        p.Set_Number_Of_Babyes(2);
                                                    }
                                                    else
                                                    {
                                                        if (uniform_variable_3 < 0.95)
                                                        {
                                                            p.Set_Number_Of_Babyes(3);
                                                        }
                                                        else
                                                        {
                                                            if (uniform_variable_3 < 0.98)
                                                            {
                                                                p.Set_Number_Of_Babyes(4);
                                                            }
                                                            else
                                                            {
                                                                p.Set_Number_Of_Babyes(5);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (p.age <= 45)
                                        {
                                            if (uniform_variable_2 < 0.4)
                                            {
                                                p.Set_Pregnancy_Month_Left(9);
                                                p.Set_Father_ID(mate.id);
                                                double uniform_variable_3 = Generate_Uniform();
                                                if (uniform_variable_3 < 0.7)
                                                {
                                                    p.Set_Number_Of_Babyes(1);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.88)
                                                    {
                                                        p.Set_Number_Of_Babyes(2);
                                                    }
                                                    else
                                                    {
                                                        if (uniform_variable_3 < 0.95)
                                                        {
                                                            p.Set_Number_Of_Babyes(3);
                                                        }
                                                        else
                                                        {
                                                            if (uniform_variable_3 < 0.98)
                                                            {
                                                                p.Set_Number_Of_Babyes(4);
                                                            }
                                                            else
                                                            {
                                                                p.Set_Number_Of_Babyes(5);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (p.age <= 60)
                                        {
                                            if (uniform_variable_2 < 0.2)
                                            {
                                                p.Set_Pregnancy_Month_Left(9);
                                                p.Set_Father_ID(mate.id);
                                                double uniform_variable_3 = Generate_Uniform();
                                                if (uniform_variable_3 < 0.7)
                                                {
                                                    p.Set_Number_Of_Babyes(1);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.88)
                                                    {
                                                        p.Set_Number_Of_Babyes(2);
                                                    }
                                                    else
                                                    {
                                                        if (uniform_variable_3 < 0.95)
                                                        {
                                                            p.Set_Number_Of_Babyes(3);
                                                        }
                                                        else
                                                        {
                                                            if (uniform_variable_3 < 0.98)
                                                            {
                                                                p.Set_Number_Of_Babyes(4);
                                                            }
                                                            else
                                                            {
                                                                p.Set_Number_Of_Babyes(5);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (p.age <= 125)
                                        {
                                            if (uniform_variable_2 < 0.05)
                                            {
                                                p.Set_Pregnancy_Month_Left(9);
                                                p.Set_Father_ID(mate.id);
                                                double uniform_variable_3 = Generate_Uniform();
                                                if (uniform_variable_3 < 0.7)
                                                {
                                                    p.Set_Number_Of_Babyes(1);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.88)
                                                    {
                                                        p.Set_Number_Of_Babyes(2);
                                                    }
                                                    else
                                                    {
                                                        if (uniform_variable_3 < 0.95)
                                                        {
                                                            p.Set_Number_Of_Babyes(3);
                                                        }
                                                        else
                                                        {
                                                            if (uniform_variable_3 < 0.98)
                                                            {
                                                                p.Set_Number_Of_Babyes(4);
                                                            }
                                                            else
                                                            {
                                                                p.Set_Number_Of_Babyes(5);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (uniform_variable_2 < 0.0075)
                                            {
                                                p.Set_Pregnancy_Month_Left(9);
                                                p.Set_Father_ID(mate.id);
                                                double uniform_variable_3 = Generate_Uniform();
                                                if (uniform_variable_3 < 0.7)
                                                {
                                                    p.Set_Number_Of_Babyes(1);
                                                }
                                                else
                                                {
                                                    if (uniform_variable_3 < 0.88)
                                                    {
                                                        p.Set_Number_Of_Babyes(2);
                                                    }
                                                    else
                                                    {
                                                        if (uniform_variable_3 < 0.95)
                                                        {
                                                            p.Set_Number_Of_Babyes(3);
                                                        }
                                                        else
                                                        {
                                                            if (uniform_variable_3 < 0.98)
                                                            {
                                                                p.Set_Number_Of_Babyes(4);
                                                            }
                                                            else
                                                            {
                                                                p.Set_Number_Of_Babyes(5);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            throw new Exception("Some error ocurred -> Imaginary boyfriend");
                        }
                    }

                    if (p.mating_desire)
                    {
                        List<Person> listToFindMate = (p.gender == Gender.Male ? femaleSubjects[year_count, month_count] : maleSubjects[year_count, month_count]);
                        
                        foreach(Person m in listToFindMate)
                        {
                            if (m.mating_desire)
                            {
                                uniform_variable_2 = Generate_Uniform();
                                if(Math.Abs(p.age - m.age) < 5)
                                {
                                    if(uniform_variable_2 < 0.45)
                                    {
                                        p.Set_Mating_Desire(false);
                                        m.Set_Mating_Desire(false);
                                        p.Set_Mate_ID(m.id);
                                        m.Set_Mate_ID(p.id);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (Math.Abs(p.age - m.age) < 10)
                                    {
                                        if (uniform_variable_2 < 0.4)
                                        {
                                            p.Set_Mating_Desire(false);
                                            m.Set_Mating_Desire(false);
                                            p.Set_Mate_ID(m.id);
                                            m.Set_Mate_ID(p.id);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (Math.Abs(p.age - m.age) < 15)
                                        {
                                            if (uniform_variable_2 < 0.35)
                                            {
                                                p.Set_Mating_Desire(false);
                                                m.Set_Mating_Desire(false);
                                                p.Set_Mate_ID(m.id);
                                                m.Set_Mate_ID(p.id);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (Math.Abs(p.age - m.age) < 20)
                                            {
                                                if (uniform_variable_2 < 0.25)
                                                {
                                                    p.Set_Mating_Desire(false);
                                                    m.Set_Mating_Desire(false);
                                                    p.Set_Mate_ID(m.id);
                                                    m.Set_Mate_ID(p.id);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (uniform_variable_2 < 0.15)
                                                {
                                                    p.Set_Mating_Desire(false);
                                                    m.Set_Mating_Desire(false);
                                                    p.Set_Mate_ID(m.id);
                                                    m.Set_Mate_ID(p.id);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        p.Set_Mating_Desire(false);
                    }

                    Person personClone = p.Clone();

                    if (p.gender == Gender.Male)
                    {
                        if (month_count == 12)
                        {
                            maleSubjects[year_count + 1, 1].Add(personClone);
                        }
                        else
                        {
                            maleSubjects[year_count, month_count + 1].Add(personClone);
                        }
                    }
                    else
                    {
                        if (month_count == 12)
                        {
                            femaleSubjects[year_count + 1, 1].Add(personClone);
                        }
                        else
                        {
                            femaleSubjects[year_count, month_count + 1].Add(personClone);
                        }
                    }
                }



                ++month_count;
                if(month_count == 13)
                {
                    ++year_count;
                    month_count = 1;
                }
            }
        }

        private void Decease(Person p, int year, int month)
        {
            p.Die();
            deceasedSubjects[year, month].Add(p);
            if(p.mate_id != "")
            {
                try
                {
                    Person exMate = usedIds[p.mate_id];

                    exMate.Set_Mate_ID("");
                    exMate.Set_Lonelyness_Time(Generate_Lonelyness_Time(exMate.age));
                }
                catch
                {
                    if (p.gender == Gender.Male)
                        throw new Exception("Some error ocurred -> Deceased man had an imaginary girlfriend");
                    if (p.gender == Gender.Female)
                        throw new Exception("Some error ocurred -> Deceased woman had an imaginary boyfriend");
                }
            }
        }

        private int Generate_Desired_Children()
        {
            int desired_children;

            double uniform_variable = Generate_Uniform();
            if (uniform_variable < 0.6)
            {
                desired_children = 1;
            }
            else
            {
                uniform_variable = Generate_Uniform();
                if (uniform_variable < 0.75)
                {
                    desired_children = 2;
                }
                else
                {
                    uniform_variable = Generate_Uniform();
                    if (uniform_variable < 0.35)
                    {
                        desired_children = 3;
                    }
                    else
                    {
                        uniform_variable = Generate_Uniform();
                        if (uniform_variable < 0.2)
                        {
                            desired_children = 4;
                        }
                        else
                        {
                            uniform_variable = Generate_Uniform();
                            if (uniform_variable < 0.1)
                            {
                                desired_children = 5;
                            }
                            else
                            {
                                uniform_variable = Generate_Uniform();
                                if (uniform_variable < 0.05)
                                {
                                    // unlimited number of children!!!
                                    desired_children = -1;
                                }
                                else
                                {
                                    desired_children = 0;
                                }
                            }
                        }
                    }
                }
            }

            return desired_children;
        }

        private string Generate_Id()
        {
            string newID = "";
            while (true)
            {
                newID = Guid.NewGuid().ToString();
                if (!usedIds.ContainsKey(newID) && newID != "")
                    break;
            }
            return newID;
        }

        // generating an int number from v1 and v2 (inclusive both) having equal probability
        private int Generate_Uniform(int v1, int v2)
        {
            int values = v2 - v1 + 1;
            double value_probability = (double)1 / (double)values;
            double[] distribution_function = new double[values + 1];
            distribution_function[0] = 0;
            for(int i = 1; i <= values; ++i)
            {
                distribution_function[i] = distribution_function[i - 1] + value_probability;
            }
            distribution_function[values] = 1;

            int value_result = 0;
            double uniform_variable = Generate_Uniform();
            for(int i = 1; i <= values; ++i)
            {
                if(uniform_variable < distribution_function[i])
                {
                    value_result = v1 + i - 1;
                    break;
                }
            }

            if(value_result == 0)
            {
                throw new Exception("Some error ocurred -> no value generated");
            }

            return value_result;
        }

        private long lastUniform;

        private void SetSeed()
        {
            Random seed = new Random();
            lastUniform = seed.Next(0, (int)m);
        }

        private long a = 16807, m;

        private void CalcM()
        {
            long result = 1;

            for(int i = 0; i < 31; ++i)
            {
                result *= 2;
            }

            m = result - 1;
        }

        private double Generate_Uniform()
        {
            lastUniform = (a * lastUniform) % m;
            return (double)lastUniform / m;
        }

        private int Generate_Lonelyness_Time(int age)
        {
            double mean = 0;

            if (age <= 15)
                mean = 3;
            else
            {
                if (age <= 21)
                    mean = 6;
                else
                {
                    if (age <= 35)
                        mean = 6;
                    else
                    {
                        if (age <= 45)
                            mean = 12;
                        else
                        {
                            if (age <= 60)
                                mean = 24;
                            else
                            {
                                if (age <= 125)
                                    mean = 48;
                                else
                                {
                                    mean = 120;
                                }
                            }
                        }
                    }
                }
            }

            return (int)(Generate_Exponential(mean));
        }

        private double Generate_Exponential(double mean)
        {
            double uniform_variable;
            do
            {
                uniform_variable = Generate_Uniform();
            } while (uniform_variable == 0d); // can't choose 0 because of the log operator
            return (-Math.Log(uniform_variable)) / mean;
        }

        public Tuple< List<Person>, List<Person>, List<Person>, List<Person> > DateInfo(int y, int m)
        {
            return new Tuple<List<Person>, List<Person>, List<Person>, List<Person>>(deceasedSubjects[y, m],
                                                                                     bornedSubjects[y, m],
                                                                                     maleSubjects[y, m],
                                                                                     femaleSubjects[y, m]);
        }
    }

    public class Person
    {
        // Common attributes
        public bool alive { get; private set; }
        public string id { get;  private set; }
        public Gender gender { get; private set; }
        public int age { get; private set; }
        public int birthday_month { get; private set; }
        public int desired_children { get; private set; }
        public int children_count { get; private set; }
        public bool mating_desire { get; private set; }
        public string mate_id { get; private set; }
        public int lonelyness_time { get; private set; }

        // Female attributes
        public int pregnancy_months_left { get; private set; }
        public int number_of_babyes { get; private set; }
        public string father_id { get; private set; }

        public Person(string id, Gender gender, int birth_month, int desired_children)
        {
            this.id = id;
            this.gender = gender;
            this.birthday_month = birth_month;
            this.desired_children = desired_children;
            alive = true;
            age = -1;
            children_count = 0;
            mating_desire = false;
            mate_id = "";
            lonelyness_time = 0;
            pregnancy_months_left = -1;
            number_of_babyes = -1;
            father_id = "";
        }

        public void Increase_Age()
        {
            ++age;
        }

        public void Increase_Children_Count(int value)
        {
            children_count += value;
        }

        public void Decrease_Lonelyness_Time()
        {
            --lonelyness_time;
        }

        public void Decrease_Pregnancy_Months_Left()
        {
            --pregnancy_months_left;
        }

        public void Set_Mating_Desire(bool value)
        {
            mating_desire = value;
        }

        public void Set_Mate_ID(string value)
        {
            mate_id = value;
        }

        public void Set_Lonelyness_Time(int value)
        {
            lonelyness_time = value;
        }

        public void Set_Pregnancy_Month_Left(int value)
        {
            pregnancy_months_left = value;
        }

        public void Set_Number_Of_Babyes(int value)
        {
            number_of_babyes = value;
        }

        public void Set_Father_ID(string value)
        {
            father_id = value;
        }

        public void Die()
        {
            alive = false;
        }

        public Person Clone()
        {
            Person result = new Person(id,gender, birthday_month, desired_children);
            result.Set_Age(age);
            result.Increase_Children_Count(children_count);
            result.Set_Mate_ID(mate_id);
            result.Set_Lonelyness_Time(lonelyness_time);
            result.Set_Pregnancy_Month_Left(pregnancy_months_left);
            result.Set_Number_Of_Babyes(number_of_babyes);
            result.Set_Father_ID(father_id);

            return result;
        }

        private void Set_Age(int age)
        {
            this.age = age;
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
