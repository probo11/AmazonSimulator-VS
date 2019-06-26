function parseCommand(input = "") {
    return JSON.parse(input);
}

var exampleSocket;

window.onload = function () {
    var startcamera, endcamera, outsidecamera, scene, renderer, baseGroup;
    var cameraControls;
    var keyboard = new THREEx.KeyboardState();
    var changeCamera = 1;
    var worldObjects = {};

    function init() {
        scene = new THREE.Scene();
        baseGroup = new THREE.Group();

        // camera link onder
        startcamera = addCamera(22, 7, 30, -10, -15, -25, 0);

        // camera rechts boven
        endcamera = addCamera(7, 7, 30, 40, -20, -40, 0);

        //buiten camera
        outsidecamera = addCamera(100, 50, -30, -10, 0, 0, 1);

        renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.setPixelRatio(window.devicePixelRatio);
        renderer.setSize(window.innerWidth, window.innerHeight + 5);
        renderer.shadowMap.enabled = true;
        renderer.shadowMap.renderReverseSided = false;
        renderer.shadowMapType = THREE.PCFSoftShadowMap;

        document.body.appendChild(renderer.domElement);

        window.addEventListener('resize', onWindowResize, false);


        // skybox
        var skyboxgeo = new THREE.SphereGeometry(500, 32, 32);
        var skyboxmat = new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("textures/background1.jpg"), side: THREE.DoubleSide });
        var skybox = new THREE.Mesh(skyboxgeo, skyboxmat);
        scene.add(skybox);

        var geometry = new THREE.BoxGeometry(20, 8, 32);
        var cubeMaterials = [
            new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load('textures/metalWall.jpg'), side: THREE.DoubleSide }), //LEFT
            new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load("textures/metalWall.jpg"), side: THREE.DoubleSide }), //RIGHT
            new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load("textures/metalFloor.jpg"), side: THREE.DoubleSide }), //top
            new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load("textures/metalFloor.jpg"), side: THREE.DoubleSide }), //bot
            new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load("textures/metalWall.jpg"), side: THREE.DoubleSide }), //front
            new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load("textures/metalWall.jpg"), side: THREE.DoubleSide }) //back
        ];

        var material = new THREE.MeshFaceMaterial(cubeMaterials);
        var plane = new THREE.Mesh(geometry, material);
        plane.position.set(14, 4, 16);
        plane.castShadow = true;
        plane.receiveShadow = true;
        baseGroup.add(plane);


        //drone laad dok ding
        geometry = new THREE.BoxGeometry(12, 8, 5); //xyz
        material = new THREE.MeshPhongMaterial({ map: new THREE.TextureLoader().load("textures/metalWall.jpg"), side: THREE.DoubleSide });
        var block = new THREE.Mesh(geometry, material);
        block.position.set(15, 4, 2.51);
        baseGroup.add(block);

        //laadcylinder
        texture = new THREE.TextureLoader().load('3dmodels/tex.jpg');
        geometry = new THREE.CylinderGeometry(2.4, 2.4, 5, 10); //xyz
        material = new THREE.MeshPhongMaterial({ map: texture });
        var cylinder = new THREE.Mesh(geometry, material);
        cylinder.position.set(15, 2.5, -2.4);
        cylinder.rotation.x = 1.6;
        //scene.add(cylinder);
        baseGroup.add(cylinder);

        // portal
        texture = new THREE.TextureLoader().load('textures/Portal.gif');
        geometry = new THREE.CylinderGeometry(25, 25, 1, 100);
        //var material = new THREE.MeshBasicMaterial({ color: 0xf0f1f2});
        material = new THREE.MeshBasicMaterial({ map: texture });
        var portal = new THREE.Mesh(geometry, material);
        portal.position.set(-150, 0, -25);
        portal.rotation.z = 1.6;

        //t is de maan
        loadOject('3dmodels/maan/', 'maan.mtl', 'maan.obj', 13, 5, 45, 60);

        // licht
        loadOject('3dmodels/light/', 'light.mtl', 'light.obj', 15, 7.2, 20, 2);
        var lamp = new THREE.PointLight(0x00ffff, 1, 30, 2); // color, intensity, distance, decay
        lamp.position.set(15, 6.1, 20);
        lamp.castShadow = true;
        baseGroup.add(lamp);

        //het is de zon
        var sunboxgeo = new THREE.SphereGeometry(20, 20, 20);
        var sunboxmat = new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("3dmodels/sun.jpg"), side: THREE.DoubleSide });
        var sunbox = new THREE.Mesh(sunboxgeo, sunboxmat);
        sunbox.position.set(0, 100, -470);
        scene.add(sunbox);

        zonlicht:
        var zonlicht = new THREE.SpotLight(0xffffff);
        zonlicht.position.set(0, 100, -470);
        zonlicht.castShadow = true;
        zonlicht.intensity = 15;
        zonlicht.distance = 550;
        zonlicht.decay = 1;
        scene.add(zonlicht);

        scene.add(baseGroup);
    }

    /**
     * Voeg een camera toe
     * @param {any} cx X positie van de camera
     * @param {any} cy Y positie van de camera
     * @param {any} cz Z positie van de camera
     * @param {any} clx X richting van de camera
     * @param {any} cly X richting van de camera
     * @param {any} clz X richting van de camera
     * @param {any} cOrbit Of de camera Orbitcontrolls heeft
     */
    function addCamera(cx, cy, cz, clx, cly, clz, cOrbit) {
        var camName = new THREE.PerspectiveCamera(70, window.innerWidth / window.innerHeight, 1, 1000);
        camName.position.x = cx;
        camName.position.y = cy;
        camName.position.z = cz;
        camName.lookAt(clx, cly, clz);
        if (cOrbit === 1) {
            cameraControls = new THREE.OrbitControls(camName);
            cameraControls.update();
        }
        scene.add(camName);
        return camName;
    }

    /**
     * Voeg een object + texture toe
     * @param {any} path Pad van het object
     * @param {any} texture texure van het object
     * @param {any} object File naam van het object
     * @param {any} ox X positie van het object
     * @param {any} oy Y positie van het object
     * @param {any} oz Z positie van het object
     * @param {any} scale Schaal van het object
     */
    function loadOject(path, texture, object, ox, oy, oz, scale) {
        new THREE.MTLLoader()
            .setPath(path)
            .load(texture, function (materials) {
                materials.preload();

                new THREE.OBJLoader()
                    .setMaterials(materials)
                    .setPath(path)
                    .load(object, function (object) {
                        object.position.set(ox, oy, oz);
                        object.scale.x = scale;
                        object.scale.y = scale;
                        object.scale.z = scale;
                        //object.castShadow = true;
                        object.receiveShadow = true;
                        baseGroup.add(object);
                    });
            });
    }

    function update() {

        if (keyboard.pressed("1")) {
            changeCamera = 1;
        }
        if (keyboard.pressed("2")) {
            changeCamera = 2;
        }
        if (keyboard.pressed("3")) {
            changeCamera = 3;
        }
    }

    function onWindowResize() {
        startcamera.aspect = window.innerWidth / window.innerHeight;
        startcamera.updateProjectionMatrix();
        renderer.setSize(window.innerWidth, window.innerHeight);
    }

    function animate() {
        requestAnimationFrame(animate);
        cameraControls.update();
        update();
        switch (changeCamera) {
            case 1:
                renderer.render(scene, startcamera);
                break;
            case 2:
                renderer.render(scene, endcamera);
                break;
            case 3:
                renderer.render(scene, outsidecamera);
                break;
            default:
                renderer.render(scene, startcamera);
                break;
        }

    }

    exampleSocket = new WebSocket("ws://" + window.location.hostname + ":" + window.location.port + "/connect_client");
    exampleSocket.onmessage = function (event) {
        var command = parseCommand(event.data);

        if (command.command === "update") {
            if (Object.keys(worldObjects).indexOf(command.parameters.guid) < 0) {
                if (command.parameters.type === "robot") {
                    var group = new THREE.Group();

                    var robot = new THREE.MTLLoader();
                    robot.setPath('3dmodels/robot/')
                        .load('materials.mtl', function (materials) {
                            materials.preload();

                            new THREE.OBJLoader()
                                .setMaterials(materials)
                                .setPath('3dmodels/robot/')
                                .load('model.obj', function (robot) {
                                    var schaal = 2;
                                    robot.scale.x = schaal;
                                    robot.scale.y = schaal;
                                    robot.scale.z = schaal;
                                    robot.castShadow = true;
                                    robot.receiveShadow = true;
                                    group.add(robot);

                                });
                        });

                    scene.add(group);
                    worldObjects[command.parameters.guid] = group;

                } else if (command.parameters.type === "fridge") {
                    group = new THREE.Group();

                    var fridge = new THREE.MTLLoader();
                    fridge.setPath('3dmodels/stellage/')
                        .load('fridge.mtl', function (materials) {
                            materials.preload();

                            new THREE.OBJLoader()
                                .setMaterials(materials)
                                .setPath('3dmodels/stellage/')
                                .load('fridge.obj', function (fridge) {
                                    var schaal = 0.05;
                                    fridge.scale.x = schaal;
                                    fridge.scale.y = schaal;
                                    fridge.scale.z = schaal;
                                    fridge.castShadow = true;
                                    fridge.receiveShadow = true;
                                    group.add(fridge);
                                });
                        });


                    scene.add(group);
                    worldObjects[command.parameters.guid] = group;

                } else if (command.parameters.type === "vrachtwagen") {
                    group = new THREE.Group();

                    var ufo = new THREE.MTLLoader();
                    ufo.setPath('3dmodels/vrachtwagen/')
                        .load('ufo.mtl', function (materials) {
                            materials.preload();

                            new THREE.OBJLoader()
                                .setMaterials(materials)
                                .setPath('3dmodels/vrachtwagen/')
                                .load('ufo.obj', function (ufo) {
                                    var schaal = 20;
                                    ufo.scale.x = schaal;
                                    ufo.scale.y = schaal;
                                    ufo.scale.z = schaal;
                                    ufo.castShadow = true;
                                    ufo.receiveShadow = true;
                                    group.add(ufo);
                                });
                        });


                    scene.add(group);
                    worldObjects[command.parameters.guid] = group;
                }
            }
        }

        var object = worldObjects[command.parameters.guid];

        object.position.x = command.parameters.x;
        object.position.y = command.parameters.y;
        object.position.z = command.parameters.z;

        object.rotation.x = command.parameters.rotationX;
        object.rotation.y = command.parameters.rotationY;
        object.rotation.z = command.parameters.rotationZ;
    };
    init();
    animate();
};
