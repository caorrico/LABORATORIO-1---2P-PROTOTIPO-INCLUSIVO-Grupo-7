using UnityEngine;
namespace AulaInclusiva
{
    public sealed class StudyCard
    {
        public readonly string Title, Text, Key;
        public StudyCard(string title, string text, string key) { Title = title; Text = text; Key = key; }
    }
    public sealed class QuizQuestion
    {
        public readonly string Prompt, Explanation;
        public readonly string[] Options;
        public readonly int Correct;
        public QuizQuestion(string prompt, int correct, string explanation, params string[] options) {
            Prompt = prompt; Correct = correct; Explanation = explanation; Options = options;
        }
    }
    public sealed class StudyLevel
    {
        public readonly string Title, Goal;
        public readonly StudyCard[] Cards;
        public readonly QuizQuestion Practice;
        public readonly QuizQuestion[] Exam;
        public StudyLevel(string title, string goal, StudyCard[] cards, QuizQuestion practice, QuizQuestion[] exam) {
            Title = title; Goal = goal; Cards = cards; Practice = practice; Exam = exam;
        }
    }
    public static class EcuadorContent
    {
        public static readonly StudyLevel[] Levels = {
            new StudyLevel("Regiones del Ecuador", "Relaciona cada región con su paisaje.", new[] {
                new StudyCard("Costa", "Está junto al océano Pacífico. Encontramos playas y llanuras.", "MAR + PLAYAS"),
                new StudyCard("Sierra / Andes", "La cordillera de los Andes atraviesa esta región. Encontramos montañas y volcanes.", "MONTAÑAS"),
                new StudyCard("Amazonía", "Está al este de los Andes. Destacan sus bosques y ríos.", "BOSQUES + RÍOS"),
                new StudyCard("Insular / Galápagos", "Es un conjunto de islas en el océano Pacífico. Forma la región Insular.", "ISLAS")
            }, new QuizQuestion("Practica: ¿qué región está formada por las islas Galápagos?", 1, "Galápagos forma la región Insular.", "Sierra", "Insular", "Amazonía"), new[] {
                new QuizQuestion("¿Cuántas regiones naturales se estudian en Ecuador?", 2, "Son cuatro: Costa, Sierra, Amazonía e Insular.", "Dos", "Tres", "Cuatro"),
                new QuizQuestion("¿En qué región se encuentra la cordillera de los Andes?", 0, "Sierra es otro nombre de la región Andina.", "Sierra", "Insular", "Costa"),
                new QuizQuestion("¿Qué región está al este de los Andes?", 1, "La Amazonía está al este de los Andes.", "Costa", "Amazonía", "Insular")
            }),
            new StudyLevel("Ciudades y lugares", "Distingue la capital y dos centros históricos.", new[] {
                new StudyCard("Quito", "Quito es la capital del Ecuador y está en los Andes. Su centro histórico es Patrimonio Mundial desde 1978.", "CAPITAL · 1978"),
                new StudyCard("Cuenca", "Cuenca está en los Andes. Su centro histórico fue inscrito como Patrimonio Mundial en 1999.", "CENTRO HISTÓRICO · 1999"),
                new StudyCard("Galápagos", "Galápagos es un archipiélago: un conjunto de islas. Es patrimonio natural, a diferencia de los centros históricos de Quito y Cuenca.", "PATRIMONIO NATURAL")
            }, new QuizQuestion("Practica: ¿cuál es la capital del Ecuador?", 0, "La capital del Ecuador es Quito.", "Quito", "Cuenca", "Galápagos"), new[] {
                new QuizQuestion("¿Cuál es la capital del Ecuador?", 1, "Quito es la capital.", "Cuenca", "Quito", "Galápagos"),
                new QuizQuestion("¿Qué centro histórico fue inscrito en 1999?", 2, "El centro histórico de Cuenca fue inscrito en 1999.", "Galápagos", "Quito", "Cuenca"),
                new QuizQuestion("¿Qué lugar es un conjunto de islas?", 0, "Galápagos es un archipiélago.", "Galápagos", "Quito", "Cuenca")
            }),
            new StudyLevel("Patrimonio cultural", "Reconoce saberes y expresiones culturales del Ecuador.", new[] {
                new StudyCard("Paja toquilla", "El tejido tradicional del sombrero ecuatoriano de paja toquilla es patrimonio cultural inmaterial. Es un saber artesanal que se transmite entre personas.", "SABER ARTESANAL"),
                new StudyCard("El pasillo", "El pasillo ecuatoriano reúne canto y poesía. Es patrimonio cultural inmaterial. Aquí lo estudiamos mediante su descripción escrita: no necesitas escuchar música.", "CANTO + POESÍA"),
                new StudyCard("Cuidar la cultura", "El patrimonio inmaterial incluye conocimientos y expresiones vivas. Conocerlos, respetar a sus comunidades y transmitir sus saberes ayuda a cuidarlos.", "CONOCER · RESPETAR · TRANSMITIR")
            }, new QuizQuestion("Practica: ¿qué saber se relaciona con la paja toquilla?", 2, "La paja toquilla se relaciona con el tejido tradicional de sombreros.", "Construir volcanes", "Formar islas", "Tejer sombreros"), new[] {
                new QuizQuestion("¿Qué se elabora tradicionalmente con paja toquilla?", 0, "Se tejen sombreros de paja toquilla.", "Sombreros", "Volcanes", "Islas"),
                new QuizQuestion("¿Qué reúne el pasillo ecuatoriano?", 2, "El pasillo reúne canto y poesía; puedes aprender sobre él leyendo.", "Solo arquitectura", "Solo geografía", "Canto y poesía"),
                new QuizQuestion("¿Qué acción ayuda a cuidar el patrimonio cultural?", 1, "Respetar y transmitir los saberes ayuda a mantenerlos vivos.", "Ignorar sus comunidades", "Respetar y transmitir saberes", "Borrar su historia")
            })
        };
    }
}
