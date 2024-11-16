TaskManagerAPI

TaskManagerAPI é uma API RESTful desenvolvida em .NET 8 para gerenciar tarefas e projetos. Este projeto visa implementar funcionalidades de criação, edição, atualização de tarefas, controle de histórico de modificações, geração de relatórios, e suporte a comentários em tarefas.

Configuração e Execução

Requisitos

	•	.NET 8 SDK
	•	Docker

	Executando a API no Docker

	Para rodar o projeto no Docker, siga os passos abaixo:
	1.	Crie a Imagem Docker:
	No terminal, navegue até a pasta raiz do projeto e execute:

	docker build -t taskmanagerapi .


	2.	Execute o Contêiner:
	Para iniciar o contêiner da API, execute o comando abaixo:

	docker run -p 5224:80 taskmanagerapi


	3.	Acessando a API:
	A API estará acessível em http://localhost:5224. Para verificar a documentação, acesse o endpoint Swagger:

	http://localhost:5224/swagger/index.html

 	4. O script para criação do banco de dados Postgres está na pasta DataBase no arquivo database.sql



Variáveis de Ambiente

Caso você precise configurar variáveis de ambiente específicas para o banco de dados, defina-as no comando docker run ou no arquivo Dockerfile conforme necessário.

Fase 2: Perguntas para Refinamento

Nesta fase, seguem as perguntas que faria ao Product Owner (PO) para esclarecer ou planejar futuras implementações e melhorias:
	
 	1.	Regras de Negócio para Comentários:
 		- Existem limitações para o número de comentários por tarefa?
    		- Comentários podem ser editados ou excluídos pelos usuários?
      
	2.	Escopo dos Relatórios de Desempenho:
		- Qual é a frequência de atualização dos relatórios de desempenho? Deve haver uma persistência dos relatórios no banco de dados?
		- Haverá necessidade de relatórios personalizados no futuro (ex.: relatórios com filtros específicos por datas, status, etc.)?
  
	3.	Controle de Acesso e Permissões:
		- Devemos considerar adicionar um sistema de autenticação no futuro, ou o sistema de identificação de usuário via payload continuará sendo suficiente?
		- Existe a intenção de adicionar papéis além de “gerente” e “usuário”?
	4.	Escalabilidade e Suporte a Mais Usuários:
		- Há um número estimado de usuários simultâneos e tarefas/projetos que o sistema deve suportar?
		- Existem requisitos específicos para auditoria de modificações nas tarefas (além do histórico)?

Fase 3:	Escalabilidade e Cloud:

	- Deploy na Nuvem: Realizar o deployment da API em provedores de nuvem como AWS, Azure ou Google Cloud Platform (GCP) para aproveitar recursos 
 	de escalabilidade, resiliência e alta disponibilidade.
  
	- Orquestração com Kubernetes: Configurar a API para rodar em um cluster Kubernetes, o que permite o gerenciamento de múltiplas réplicas 
 	da aplicação e facilita o autoescalonamento de acordo com a demanda. Isso ajudará a gerenciar o balanceamento de carga e garantir que a API responda adequadamente ao aumento de tráfego.
  
	- Implementação de Cache Distribuído: Utilizar uma camada de cache, como Redis ou Memcached, para armazenar dados acessados com frequência, 
 	como resultados de relatórios. Isso ajudará a reduzir a carga sobre o banco de dados e a melhorar a velocidade de resposta da API.
  
	- Configuração de CI/CD: Implementar um pipeline de CI/CD com ferramentas como GitHub Actions, GitLab CI ou Azure DevOps 
 	para automatizar a integração e entrega contínua. Esse pipeline incluirá etapas de testes, build e deployment, facilitando a implantação de atualizações de forma confiável e eficiente.
  
	- Monitoramento e Logging Centralizado: Utilizar ferramentas de monitoramento como Prometheus e Grafana para acompanhar a performance e disponibilidade da API. 
 	Além disso, configurar um sistema de logging centralizado com o ELK Stack (Elasticsearch, Logstash e Kibana) ou AWS CloudWatch para coletar e analisar logs, 
  	facilitando a identificação de problemas e gargalos no sistema.
   
	- Políticas de Autoescalabilidade e Recuperação de Falhas: Configurar autoescalabilidade no cluster de Kubernetes ou na infraestrutura de nuvem para aumentar ou reduzir a quantidade de recursos automaticamente com base na demanda. 		- Implementar práticas de recuperação automática de falhas para garantir a alta disponibilidade da API e reduzir o tempo de inatividade.
 
    
