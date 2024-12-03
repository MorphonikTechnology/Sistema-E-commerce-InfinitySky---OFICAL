Create Database bdInfinity; -- creation the database of Infinity Sky enterprise
USE bdInfinity; -- using database --

-- TABELAS --
-- PAÍS --
Create table Pais_tbl(
	id_pais int primary key auto_increment,
    nome_pais varchar(50) not null,
    clima_pais varchar(50) not null,
    comidas_tip varchar(50) not null,
    moeda_pais varchar(20) not null,
    descricao_pais varchar(800) not null,
    image_pais varchar (50) not null,
	image_clima varchar (50) not null,
	image_comida varchar (50) not null, 
    image_moeda varchar (50) not null 
);

-- PLANO --
-- alteração na tabela plano (novo campo parcela e qtd) --
Create table Plano_tbl(
    id_plano int primary key auto_increment,
    nome_plano varchar (100) not null,
    hospedagem_plano varchar(100) not null,
    curso_plano varchar(100) not null,
    instituicao_plano varchar(150) not null,
    periodo_plano varchar(20) not null,
    descricao_plano varchar(800) not null,
    image_plano varchar(255),
    id_pais int not null,
    valor decimal(8,2) NOT NULL,
    parcela_plano varchar (100) not null,
    qtd_plano int,
    constraint Fk_id_pais foreign key (id_pais) references Pais_tbl(id_pais)
);

-- TELEFONE --
create table Telefone_tbl(
	id_telefone int primary key auto_increment,
	telefone_cliente varchar(11) not null
);
-- insert adm --
INSERT INTO Telefone_tbl (telefone_cliente) VALUES ('11999999999');


-- CLIENTE --
-- tabela cliente oficial com fk na telefone --
create table Cliente_tbl(
	id_cliente int primary key auto_increment,
    nome_cliente varchar(100) not null,
    email_cliente varchar(150) not null,
    senha_cliente varchar(50) not null,
    cpf_cliente varchar(11) not null,
    id_telefone int not null,
    data_nascimento date not null,
    constraint FK_id_telefone_cliente foreign key (id_telefone) references Telefone_tbl(id_telefone),
    constraint UC_cpf_cliente unique (cpf_cliente)
);
-- insert adm --
INSERT INTO Cliente_tbl (id_cliente, nome_cliente, email_cliente, senha_cliente, cpf_cliente, id_telefone, data_nascimento) VALUES (1, 'Administrador', 'adm@gmail.com', 'adm123', '00000000000', 1,'1990-01-01');

-- CARRINHO --
create table Carrinho_tbl(
	id_carrinho int primary key auto_increment,
    itens_carrinho int not null,
    valor_total_carrinho decimal(8,2) not null,
    id_plano int not null,
    id_cliente int not null,
    constraint FK_id_plano_carrinho foreign key (id_plano) references Plano_tbl(id_plano),
    constraint FK_id_cliente_carrinho foreign key (id_cliente) references Cliente_tbl(id_cliente)
);

-- FAVORITOS --
create table Favorito_tbl(
	id_favorito int primary key auto_increment,
    status_favorito varchar(10) not null,
    id_plano int not null,
    id_cliente int not null,
    constraint FK_id_plano_favorito foreign key (id_plano) references Plano_tbl(id_plano),
    constraint FK_id_cliente_favorito foreign key (id_cliente) references Cliente_tbl(id_cliente)
);

-- PAGAMENTO --
create table Pagamento_tbl(
	id_pagamento int primary key auto_increment,
    forma_pagamento varchar(10) not null,
    status_pagamentos varchar(10) not null,
    hora_pagamento time not null,
    valor_pagamento decimal(8,2) not null,
    id_carrinho int not null,
    constraint FK_id_carrinho_pag foreign key (id_carrinho) references Carrinho_tbl(id_carrinho)
);

-- NOTA FISCAL --
create table NF_tbl(
	id_nf int primary key auto_increment,
    valor_nf decimal (8,2),
    data_emissao date not null,
    hora_emissao time not null,
    id_pagamento int not null,
    constraint FK_id_pagamento_nf foreign key (id_pagamento) references Pagamento_tbl(id_pagamento)
);

-- INSERTS NAS TABELAS (PAÍSES E PLANOS) --

-- inserts dos 13 países parceiros da InfinitySky --
INSERT INTO Pais_tbl (id_pais, nome_pais, clima_pais, comidas_tip, moeda_pais, descricao_pais, image_pais, image_clima, image_comida, image_moeda)
VALUES 
(DEFAULT, 'Canadá', 'Temperado, Ártico', 'Poutine; Maple Syrup; Tourtière', 'Dólar Canadense', 'O Canadá é o segundo maior país do mundo, com paisagens diversas como montanhas, florestas e lagos. É conhecido por sua multiculturalidade, cidades como Toronto, Vancouver e Montreal, e seu alto padrão de vida.', '/Paises/canada.png', '/ImagensInfo/canadaclima.png', '/ImagensInfo/canadacomida.png','/ImagensInfo/canadamoeda.png'),
(DEFAULT, 'Portugal', 'Mediterrâneo', 'Bacalhau; Pastel de Nata; Sardinhas', 'Euro', 'Portugal é um país no sul da Europa conhecido por suas praias, cidades históricas como Lisboa e Porto, e pela sua rica herança cultural e culinária.', '/Paises/portugal.png', '/ImagensInfo/portugalclima.png', '/ImagensInfo/portugalcomida.png','/ImagensInfo/portugalmoeda.png'),
(DEFAULT, 'EUA', 'Variado', 'Hambúrguer; Hot Dog; BBQ', 'Dólar Americano', 'Os Estados Unidos são uma das maiores economias e destinos turísticos do mundo, com cidades icônicas como Nova York, Los Angeles e Chicago, além de grande diversidade cultural e geográfica.', '/Paises/eua.png', '/ImagensInfo/euaclima.png', '/ImagensInfo/euacomida.png','/ImagensInfo/euamoeda.png'),
(DEFAULT, 'Argentina', 'Temperado', 'Churrasco; Empanada; Alfajor', 'Peso Argentino', 'A Argentina é famosa por sua cultura vibrante, com destaque para o tango e a paixão pelo futebol. É um dos maiores países da América do Sul, com atrações como Buenos Aires, as Cataratas do Iguaçu e a Patagônia.', '/Paises/argentina.png', '/ImagensInfo/argentinaclima.png', '/ImagensInfo/argentinacomida.png','/ImagensInfo/argentinamoeda.png'),
(DEFAULT, 'Itália', 'Mediterrâneo', 'Pizza; Lasanha; Spaghetti', 'Euro', 'A Itália é um país localizado no sul da Europa, famoso por sua rica história, arte e cultura, com cidades icônicas como Roma, Florença e Veneza. É um destino popular para turismo, oferecendo uma mistura de patrimônio histórico e belezas naturais.', '/Paises/italia.png', '/ImagensInfo/italiaclima.png', '/ImagensInfo/italiacomida.png','/ImagensInfo/italiamoeda.png'),
(DEFAULT, 'Espanha', 'Mediterrâneo', 'Paella; Gazpacho; Tortilla', 'Euro', 'A Espanha é conhecida por suas cidades vibrantes como Madrid e Barcelona, seu clima variado, culinária de renome mundial e sua cultura rica, repleta de festas tradicionais.', '/Paises/espanha.png', '/ImagensInfo/espanhaclima.png', '/ImagensInfo/espanhacomida.png','/ImagensInfo/espanhamoeda.png'),
(DEFAULT, 'Alemanha', 'Temperado', 'Salsicha; Chucrute; Pretzel', 'Euro', 'A Alemanha é uma potência econômica da Europa, com cidades históricas como Berlim, Munique e Frankfurt. Conhecida por sua eficiência e tecnologia, também oferece uma rica herança cultural e paisagens belíssimas.', '/Paises/alemanha.png', '/ImagensInfo/alemanhaclima.png', '/ImagensInfo/alemanhacomida.png','/ImagensInfo/alemanhamoeda.png'),
(DEFAULT, 'Austrália', 'Tropical', 'Vegemite; Pavlova; Carneiro Assado', 'Dólar Australiano', 'A Austrália é um continente e país no hemisfério sul, conhecido por suas praias, florestas e desertos, além de sua vida selvagem única. As principais cidades incluem Sydney, Melbourne e Brisbane.', '/img/australia.png', '/ImagensInfo/australiaclima.png', '/ImagensInfo/australiacomida.png','/ImagensInfo/australiamoeda.png'),
(DEFAULT, 'Inglaterra', 'Temperado', 'Fish and Chips; Roast Beef; Shepherd\'s Pie', 'Libra Esterlina', 'Inglaterra é parte do Reino Unido, famosa por sua história rica, instituições renomadas como a Universidade de Oxford, e cidades como Londres e Manchester.', '/Paises/inglaterra.png', '/ImagensInfo/inglaterraclima.png', '/ImagensInfo/inglaterracomida.png','/ImagensInfo/inglaterramoeda.png'),
(DEFAULT, 'França', 'Mediterrâneo, Oceânico', 'Croissant; Baguette; Ratatouille', 'Euro', 'A França é um dos destinos turísticos mais populares do mundo, conhecida por sua história, arte e culinária. Paris, a capital, é um centro global de cultura e moda.', '/Paises/franca.png', '/ImagensInfo/francaclima.png', '/ImagensInfo/francacomida.png','/ImagensInfo/francamoeda.png'),
(DEFAULT, 'Irlanda', 'Oceânico', 'Irish Stew; Soda Bread; Boxty', 'Euro', 'A Irlanda é conhecida por sua paisagem verdejante, cidades históricas como Dublin e Cork, e sua cultura vibrante, marcada pela música tradicional e literatura.', '/Paises/irlanda.png', '/ImagensInfo/irlandaclima.png', '/ImagensInfo/irlandacomida.png','/ImagensInfo/irlandamoeda.png'),
(DEFAULT, 'Japão', 'Subtropical, Temperado', 'Sushi; Ramen; Tempura', 'Iene', 'O Japão é uma nação insular no leste da Ásia, famosa por sua tecnologia avançada, cultura tradicional e cidades vibrantes como Tóquio, Kyoto e Osaka.', '/Paises/japao.png', '/ImagensInfo/japaoclima.png', '/ImagensInfo/japaocomida.png','/ImagensInfo/japaomoeda.png'),
(DEFAULT, 'Coreia do Sul', 'Subtropical', 'Kimchi; Bibimbap; Bulgogi', 'Won', 'A Coreia do Sul é um país altamente desenvolvido, conhecido por sua indústria tecnológica, música pop (K-pop) e cultura rica. Cidades como Seul e Busan são grandes centros culturais e econômicos.','/Paises/coreia.png', '/ImagensInfo/coreiaclima.png', '/ImagensInfo/coreiacomida.png','/ImagensInfo/coreiamoeda.png');

-- inserts dos planos desses países --
-- inserts do canadá --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Vancouver - 6 Meses', 'Homestay', 'Inglês', 'ILAC Vancouver', '6 meses', 'Curso intensivo de inglês em Vancouver na ILAC, uma das escolas mais renomadas do Canadá.', '/Imagens/planocanada1.png', 1, 12000.00, 'Em até 12x de R$1000.00', 5),
('Toronto - 4 Meses', 'Residência Estudantil', 'Inglês', 'EC Toronto', '4 meses', 'Estudo intensivo de inglês na EC Toronto, localizada no coração da cidade.', '/Imagens/planocanada2.png', 1, 15000.00, 'Em até 12x de R$1250.00', 6),
('Montreal - 8 Meses', 'Homestay', 'Inglês e Francês', 'LSC Montreal', '8 meses', 'Curso bilíngue de inglês e francês em Montreal, oferecido pela LSC.', '/Imagens/planocanada3.png', 1, 18000.00, 'Em até 12x de R$1500.00', 7);
				
-- Portugal --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Lisboa - 6 Meses', 'Homestay', 'Português', 'Lusa Language School', '6 meses', 'Curso intensivo de português em Lisboa, na Lusa Language School.', '/Imagens/planoportugal1.png', 2, 10000.00, 'Em até 12x de R$833.33', 1),
('Porto - 3 Meses', 'Residência Estudantil', 'Português', 'Fast Forward Language Institute', '3 meses', 'Estudo intensivo de português em Porto, na Fast Forward, a melhor Universidade da região.', '/Imagens/planoportugal2.png', 2, 8000.00, 'Em até 12x de R$666.67', 1),
('Lisboa - 1 Ano', 'Homestay', 'Português', 'CIAL Centro de Línguas', '1 ano', 'Curso avançado de português em Lisboa, oferecido pelo CIAL Centro de Línguas.', '/Imagens/planoportugal3.png', 2, 20000.00, 'Em até 12x de R$1666.67', 1);
				
-- Estados Unidos --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Nova York - 6 Meses', 'Residência Estudantil', 'Inglês', 'Kaplan International NY', '6 meses', 'Curso de inglês em Nova York, oferecido pela Kaplan International.', '/Imagens/planoestadosunidos1.png', 3, 18000.00, 'Em até 12x de R$1500.00', 1),
('Los Angeles - 4 Meses', 'Homestay', 'Inglês', 'EC Los Angeles', '4 meses', 'Curso intensivo de inglês na EC Los Angeles, Universidade coração da Cidade.', '/Imagens/planoestadosunidos2.png', 3, 16000.00, 'Em até 12x de R$1333.33', 1),
('Miami - 1 Ano', 'Homestay', 'Inglês', 'ELS Language Centers Miami', '1 ano', 'Estudo de inglês de longa duração no ELS Language Centers, em Miami.', '/Imagens/planoestadosunidos3.png', 3, 30000.00, 'Em até 12x de R$2500.00', 1);

-- Argentina --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Buenos Aires - 6 Meses', 'Homestay', 'Espanhol', 'Academia Buenos Aires', '6 meses', 'Curso intensivo de espanhol em Buenos Aires, oferecido pela Academia Buenos Aires.', '/Imagens/planoargentina1.png', 4, 10000.00, 'Em até 12x de R$833.33', 1),
('Córdoba - 3 Meses', 'Residência Estudantil', 'Espanhol', 'Set Idiomas Córdoba', '3 meses', 'Curso de espanhol em Córdoba, uma das cidades mais vibrantes da Argentina.', '/Imagens/planoargentina2.png', 4, 8000.00, 'Em até 12x de R$666.67', 1),
('Mendoza - 1 Ano', 'Homestay', 'Espanhol', 'Intercultural Mendoza', '1 ano', 'Curso avançado de espanhol em Mendoza, cidade conhecida por seus vinhos e montanhas.', '/Imagens/planoargentina3.png', 4, 15000.00, 'Em até 12x de R$1250.00', 1);

-- Italia --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Roma - 6 Meses', 'Homestay', 'Italiano', 'Scuola Leonardo da Vinci', '6 meses', 'Curso intensivo de italiano em Roma, oferecido pela Scuola Leonardo da Vinci.', '/Imagens/planoitalia1.png', 5, 12000.00, 'Em até 12x de R$1000.00', 1),
('Florença - 4 Meses', 'Residência Estudantil', 'Italiano', 'Centro Fiorenza', '4 meses', 'Curso intensivo de italiano em Florença, cidade do Renascimento.', '/Imagens/planoitalia2.png', 5, 10000.00, 'Em até 12x de R$833.33', 1),
('Milão - 1 Ano', 'Homestay', 'Italiano', 'Scuola Italiana Milano', '1 ano', 'Curso de italiano de longa duração em Milão, centro da moda e design.', '/Imagens/planoitalia3.png', 5, 18000.00, 'Em até 12x de R$1500.00', 1);

-- Espanha-- 
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Barcelona - 6 Meses', 'Homestay', 'Espanhol', 'Don Quijote Barcelona', '6 meses', 'Curso intensivo de espanhol em Barcelona, oferecido pela Don Quijote.', '/Imagens/planoespanha1.png', 6, 11000.00, 'Em até 12x de R$916.67', 1),
('Madri - 3 Meses', 'Residência Estudantil', 'Espanhol', 'Enforex Madrid', '3 meses', 'Curso intensivo de espanhol em Madri, a capital da Espanha.', '/Imagens/planoespanha2.png', 6, 9000.00, 'Em até 12x de R$750.00', 1),
('Sevilha - 1 Ano', 'Homestay', 'Espanhol', 'Clic International House', '1 ano', 'Curso avançado de espanhol em Sevilha, cidade histórica da Andaluzia.', '/Imagens/planoespanha3.png', 6, 20000.00, 'Em até 12x de R$1666.67', 1);

-- Alemanha -- 
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Berlim - 6 Meses', 'Homestay', 'Alemão', 'Goethe-Institut Berlim', '6 meses', 'Curso intensivo de alemão no Goethe-Institut em Berlim.', '/Imagens/planoalemanha1.png', 7, 15000.00, 'Em até 12x de R$1250.00', 1),
('Munique - 4 Meses', 'Residência Estudantil', 'Alemão', 'DID Deutsch-Institut Munique', '4 meses', 'Curso de alemão em Munique, centro cultural da Baviera.', '/Imagens/planoalemanha2.png', 7, 12000.00, 'Em até 12x de R$1000.00', 1),
('Frankfurt - 1 Ano', 'Homestay', 'Alemão', 'Sprachcaffe Frankfurt', '1 ano', 'Curso avançado de alemão em Frankfurt, cidade financeira da Alemanha.', '/Imagens/planoalemanha3.png', 7, 22000.00, 'Em até 12x de R$1833.33', 1);

-- Austrália --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Sydney - 6 Meses', 'Homestay', 'Inglês', 'Navitas English Sydney', '6 meses', 'Curso intensivo de inglês em Sydney, oferecido pela Navitas.', '/Imagens/planoaustralia1.png', 8, 14000.00, 'Em até 12x de R$1166.67', 1),
('Melbourne - 4 Meses', 'Residência Estudantil', 'Inglês', 'IH Melbourne', '4 meses', 'Curso de inglês em Melbourne, uma das cidades mais dinâmicas da Austrália.', '/Imagens/planoaustralia2.png', 8, 12000.00, 'Em até 12x de R$1000.00', 1),
('Brisbane - 1 Ano', 'Homestay', 'Inglês', 'Langports Brisbane', '1 ano', 'Curso intensivo de inglês em Brisbane, cidade com clima tropical e praias.', '/Imagens/planoaustralia3.png', 8, 25000.00, 'Em até 12x de R$2083.33', 1);

-- Inglaterra --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES 
('Londres - 6 Meses', 'Homestay', 'Inglês', 'The London School of English', '6 meses', 'Curso de inglês em Londres, uma das cidades mais famosas do mundo.', '/Imagens/planoinglaterra1.png', 9, 18000.00, 'Em até 12x de R$1500.00', 1),
('Manchester - 4 Meses', 'Residência Estudantil', 'Inglês', 'EC Manchester', '4 meses', 'Curso intensivo de inglês em Manchester.', '/Imagens/planoinglaterra2.png', 9, 14000.00, 'Em até 12x de R$1166.67', 1),
('Brighton - 1 Ano', 'Homestay', 'Inglês', 'EF Brighton', '1 ano', 'Curso avançado de inglês em Brighton, cidade costeira popular entre estudantes.', '/Imagens/planoinglaterra3.png', 9, 25000.00, 'Em até 12x de R$2083.33', 1);

-- França --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES
('Paris - 6 Meses', 'Homestay', 'Francês', 'Alliance Française Paris', '6 meses', 'Curso de francês em Paris, oferecido pela renomada Alliance Française.', '/Imagens/planofranca1.png', 10, 16000.00, 'Em até 12x de R$1333.33', 1),
('Lyon - 3 Meses', 'Residência Estudantil', 'Francês', 'Inflexyon Lyon', '3 meses', 'Curso intensivo de francês em Lyon, famosa por sua gastronomia e cultura.', '/Imagens/planofranca2.png', 10, 12000.00, 'Em até 12x de R$1000.00', 1),
('Nice - 1 Ano', 'Homestay', 'Francês', 'Azurlingua', '1 ano', 'Curso de francês avançado em Nice, cidade costeira do sul da França.', '/Imagens/planofranca3.png', 10, 24000.00, 'Em até 12x de R$2000.00', 1);

-- Irlanda --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES
('Dublin - 6 Meses', 'Homestay', 'Inglês', 'Dublin School of English', '6 meses', 'Curso intensivo de inglês em Dublin, oferecido pela Dublin School of English.', '/Imagens/planoirlanda1.png', 11, 13000.00, 'Em até 12x de R$1083.33', 1),
('Cork - 4 Meses', 'Residência Estudantil', 'Inglês', 'Cork English Academy', '4 meses', 'Curso de inglês em Cork, cidade cultural da Irlanda.', '/Imagens/planoirlanda2.png', 11, 11000.00, 'Em até 12x de R$916.67', 1),
('Galway - 1 Ano', 'Homestay', 'Inglês', 'Atlantic Language Galway', '1 ano', 'Curso de inglês avançado em Galway, conhecida por sua música e festivais.', '/Imagens/planoirlanda3.png', 11, 22000.00, 'Em até 12x de R$1833.33', 1);

-- Japão --
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES
('Tóquio - 6 Meses', 'Homestay', 'Japonês', 'Kudan Institute of Japanese Language', '6 meses', 'Curso intensivo de japonês em Tóquio, oferecido pelo Kudan Institute.', '/Imagens/planojapao1.png', 12, 18000.00, 'Em até 12x de R$1500.00', 1),
('Quioto - 3 Meses', 'Residência Estudantil', 'Japonês', 'GenkiJACS Kyoto', '3 meses', 'Curso de japonês em Quioto, antiga capital do Japão e berço da cultura tradicional.', '/Imagens/planojapao2.png', 12, 15000.00, 'Em até 12x de R$1250.00', 1),
('Osaka - 1 Ano', 'Homestay', 'Japonês', 'Osaka Japanese Language Academy', '1 ano', 'Curso avançado de japonês em Osaka, cidade vibrante e moderna do Japão.', '/Imagens/planojapao3.png', 12, 28000.00, 'Em até 12x de R$2333.33', 1);

-- Coreia do sul--
INSERT INTO Plano_tbl (nome_plano, hospedagem_plano, curso_plano, instituicao_plano, periodo_plano, descricao_plano, image_plano, id_pais, valor, parcela_plano, qtd_plano)
VALUES
('Seul - 6 Meses', 'Homestay', 'Coreano', 'Korea University Sejong Campus', '6 meses', 'Curso intensivo de coreano em Seul, oferecido pela Korea University.', '/Imagens/planocoreia1.png', 13, 17000.00, 'Em até 12x de R$1416.67', 1),
('Busan - 4 Meses', 'Residência Estudantil', 'Coreano', 'Pusan National University', '4 meses', 'Curso de coreano em Busan, cidade costeira famosa por suas praias.', '/Imagens/planocoreia2.png', 13, 14000.00, 'Em até 12x de R$1166.67', 1),
('Incheon - 1 Ano', 'Homestay', 'Coreano', 'Inha University', '1 ano', 'Curso avançado de coreano em Incheon, cidade moderna próxima a Seul.', '/Imagens/planocoreia3.png', 13, 25000.00, 'Em até 12x de R$2083.33', 1);


-- PROCEDURES --
-- procedure que separa os planos em sessões --
Delimiter //
create procedure SelecionarPlanosPorPaisIdsO(IN PaisId INT)
begin
    select
        p.id_plano,
        p.nome_plano, 
        p.hospedagem_plano, 
        p.curso_plano, 
        p.instituicao_plano, 
        p.periodo_plano, 
        p.descricao_plano, 
        p.image_plano, 
		p.id_pais,
        p.valor, 
        p.parcela_plano, 
        p.qtd_plano,
        pa.nome_pais
    from 
        Plano_tbl p
    inner join
        Pais_tbl pa ON p.id_pais = pa.id_pais
    where 
        pa.id_pais = PaisId;
end //
Delimiter ;

-- procedure que puxa os 3 primeiros planos cadastrados --
Delimiter $$
create procedure SelecionarTresPrimeirosPlanos()
begin
    select *
    from Plano_tbl
    order by id_plano ASC -- ordena por ordem de inserção, ou seja, pega o id --
    limit 3; -- retorna os 3 primeiros --
end$$
Delimiter ;
call SelecionarTresPrimeirosPlanos;

-- procedure que pega o restante dos planos, os 6 depois dos primeiros 3 inserts --
Delimiter //
create procedure SelecionarSeisPlanos()
begin
    select *
    from Plano_tbl
    order by id_plano
    limit 6 offset 3;
end //
Delimiter ;
call SelecionarSeisPlanos();


-- procedures de verificação dos clientes, obter dados --
delimiter //
create procedure ObtendoDadosClientes(
    in VClienteId int
)
begin
    select C.Nome, C.DataNascimento, C.Email, C.CPF, T.NumeroTelefone
    from Cliente C
    join Telefone T ON C.ClienteId = T.ClienteId
    where C.ClienteId = VClienteId;
end //
Delimiter ;

-- procedure que atualiza os dados dos clientes, chama pra atualização --
Delimiter //
create procedure AtualizandoDadosClienteOFC(
    IN VClienteId INT,
    IN VNome VARCHAR(100),
    IN VDataNascimento DATE,
    IN VEmail VARCHAR(100),
    IN VCPF VARCHAR(20),
    IN VNumeroTelefone VARCHAR(20)
)
begin
    UPDATE Cliente
    SET Nome = VNome, DataNascimento = VDataNascimento, Email = VEmail, CPF = VCPF
    WHERE ClienteId = VClienteId;

    UPDATE Telefone
    SET NumeroTelefone = VNumeroTelefone
    WHERE ClienteId = VClienteId;
end //
Delimiter ;


-- DESCRIBES --                        
describe Pais_tbl;
describe Plano_tbl;
describe Cliente_tbl;
describe Carrinho_tbl;
describe Favorito_tbl;
describe Pagamento_tbl;
describe Telefone_tbl;
describe NF_tbl;
describe HistoricoCarrinho;
describe HistoricoFavorito;
describe HistoricoCliente;
describe HistoricoPagamento;

-- SELECTS --                        
select * from Pais_tbl;
select * from Plano_tbl;
select * from Cliente_tbl;
select * from Carrinho_tbl;
select * from Favorito_tbl;
select * from Pagamento_tbl;
select * from Telefone_tbl;
select * from NF_tbl;

-- SHOWS --                        
show databases;
show tables;
show variables like "sql_safe_updates";

-- FUNÇÕES DE TEMPO, SUER, DATA... --                        
select current_date();
select current_time();
select current_user();
set sql_safe_updates = 0;

-- DROPS --                        
drop database bd_infinity;
drop table Pais_tbl;
drop table Plano_tbl;
drop table Cliente_tbl;
drop table Carrinho_tbl;
drop table Favorito_tbl;
drop table Pagamento_tbl;
drop table Telefone_tbl;
drop table NF_tbl;
drop procedure spInsertCliente;
drop view Vw_Planos;