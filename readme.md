# Performance Aware Shooter

# Jobs

I took advantage of scheduled jobs as much as possible. Both regular and parallel jobs are used on which one was required for the game to run properly. In some instances it was quite unclear why I sometimes had to use one or the other, even though I tried to find specific information through the documentation. Either way, the performance is quite good so I might be overthinking it.

# Authoring

I chose to prepare prefabs for all of my objects, adding authoring for those that required specific components. I learned that some people choose to not use the authoring/baking system at all to avoid issues with translating the GameObject architecture into ECS. This is something I might look into further in the future. E.g. gameobjects are often quite hierarchical with children, while ecs tends to avoid adding child-entities unless absolutely necessary.

## Visual example of the text above
```
GameObject:
    Player [PlayerController, Rigidbody, Collider]
    |   Player Model [Mesh Renderer, Mesh Filter]
    |   Player Weapon [Weapon]
        |   Weapon Model [Mesh Renderer, Mesh Filter]

Entity:
    Player [PlayerController, RigidBody, Collider, Mesh Renderer, Mesh Filter]
    |   Player Weapon [Weapon, Mesh Renderer, Mesh Filter]
```

# Burst
I chose to put the [BurstCompile] attribute on all of my systems and their `OnUpdate` function, as well as my Jobs and their `Execute` function. It seems to me like there is no real negative impact on compile times from doing it in a place where it isnt effective, and unity doesn't let you know when it does not change anything. This is a usability nightmare, and seems like a really easy way to mess up your dev-experience since Burst mangles the code and doesn't provide usable exceptions. For me, the [BurstCompile] attribute seems like a way to increase performance after the code has already been written, and can be a really quick way to prematurely optimize your code. 

# Managed code

## Input system

The system that handles input has to interface with the generated InputActions class. This requires a member-scope instance (pointer) which is why the system is a class and inherits from SystemBase. ISystem structs do not support managed fields.

## Camera System

Unity DOTS does not support camera entities. This is why the code has to interface with the default Unity transform instead.