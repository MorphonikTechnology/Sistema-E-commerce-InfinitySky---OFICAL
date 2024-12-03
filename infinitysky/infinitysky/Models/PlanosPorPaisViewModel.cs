namespace infinitysky.Models
{
    public class PlanosPorPaisViewModel
    {

        public IEnumerable<Planos> PlanosCanadá { get; set; }

        public IEnumerable<Planos> PlanosPortugal { get; set; }


        public IEnumerable<Planos> PlanosEstadosUnidos { get; set; }

        public IEnumerable<Planos> PlanosArgentina { get; set; }

        public IEnumerable<Planos> PlanosItalia { get; set; }

        public IEnumerable<Planos> PlanosEspanha { get; set; }

        public IEnumerable<Planos> PlanosAlemanha { get; set; }

        public IEnumerable<Planos> PlanosAustrália { get; set; }

        public IEnumerable<Planos> PlanosInglaterra { get; set; }

        public IEnumerable<Planos> PlanosFrança { get; set; }

        public IEnumerable<Planos> PlanosIrlanda { get; set; }

        public IEnumerable<Planos> PlanosJapão { get; set; }

        public IEnumerable<Planos> PlanosCoreiadoSul { get; set; }
        public Pais Canada { get; set; }
        public Pais Portugal { get; set; }
        public Pais EUA { get; set; }
        public Pais Argentina { get; set; }
        public Pais Italia { get; set; }
        public Pais Alemanha { get; set; }
        public Pais Australia { get; set; }
        public Pais Inglaterra { get; set; }
        public Pais Franca { get; set; }
        public Pais Irlanda { get; set; }
        public Pais Japao { get; set; }
        public Pais Coreia { get; set; }
        public Pais Espanha { get; set; }

        public List<Planos> Planos { get; set; }
        public List<Pais> Pais { get; set; }
    }
}
