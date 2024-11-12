-- DROP SCHEMA task_manager;

CREATE SCHEMA task_manager AUTHORIZATION postgres;


-- task_manager."user" definition

-- Drop table

-- DROP TABLE task_manager."user";

CREATE TABLE task_manager."user" (
	id serial4 NOT NULL,
	"name" varchar(100) NOT NULL,
	email varchar(100) NOT NULL,
	passwordhash varchar(255) NOT NULL,
	createdat timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	CONSTRAINT user_email_key UNIQUE (email),
	CONSTRAINT user_pkey PRIMARY KEY (id)
);


-- task_manager.project definition

-- Drop table

-- DROP TABLE task_manager.project;

CREATE TABLE task_manager.project (
	id serial4 NOT NULL,
	userid int4 NOT NULL,
	"name" varchar(100) NOT NULL,
	description text NULL,
	startdate timestamp NOT NULL,
	status varchar(20) DEFAULT 'Active'::character varying NULL,
	enddate timestamp NULL,
	CONSTRAINT project_pkey PRIMARY KEY (id),
	CONSTRAINT project_status_check CHECK (((status)::text = ANY ((ARRAY['Active'::character varying, 'Completed'::character varying, 'Cancelled'::character varying, 'Deleted'::character varying])::text[]))),
	CONSTRAINT project_user_id_fkey FOREIGN KEY (userid) REFERENCES task_manager."user"(id)
);


-- task_manager.todo_task definition

-- Drop table

-- DROP TABLE task_manager.todo_task;

CREATE TABLE task_manager.todo_task (
	id serial4 NOT NULL,
	projectid int4 NOT NULL,
	title varchar(100) NOT NULL,
	description text NULL,
	createdat timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	duedate timestamp NOT NULL,
	priority varchar(10) DEFAULT 'Medium'::character varying NULL,
	status varchar(20) DEFAULT 'Pending'::character varying NULL,
	CONSTRAINT todo_task_pkey PRIMARY KEY (id),
	CONSTRAINT todo_task_priority_check CHECK (((priority)::text = ANY ((ARRAY['Low'::character varying, 'Medium'::character varying, 'High'::character varying])::text[]))),
	CONSTRAINT todo_task_status_check CHECK (((status)::text = ANY ((ARRAY['Pending'::character varying, 'InProgress'::character varying, 'Completed'::character varying, 'Cancelled'::character varying, 'Deleted'::character varying])::text[]))),
	CONSTRAINT todo_task_project_id_fkey FOREIGN KEY (projectid) REFERENCES task_manager.project(id)
);


-- task_manager."comments" definition

-- Drop table

-- DROP TABLE task_manager."comments";

CREATE TABLE task_manager."comments" (
	id serial4 NOT NULL,
	taskid int4 NOT NULL,
	userid int4 NOT NULL,
	commenttext text NOT NULL,
	createdat timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	CONSTRAINT comments_pkey PRIMARY KEY (id),
	CONSTRAINT comments_task_id_fkey FOREIGN KEY (taskid) REFERENCES task_manager.todo_task(id)
);


-- task_manager.history definition

-- Drop table

-- DROP TABLE task_manager.history;

CREATE TABLE task_manager.history (
	id serial4 NOT NULL,
	taskid int4 NOT NULL,
	description text NOT NULL,
	modifiedat timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	userid int4 NOT NULL,
	CONSTRAINT history_pkey PRIMARY KEY (id),
	CONSTRAINT history_task_id_fkey FOREIGN KEY (taskid) REFERENCES task_manager.todo_task(id)
);